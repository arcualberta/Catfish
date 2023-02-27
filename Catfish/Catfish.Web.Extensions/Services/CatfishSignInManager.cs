using CatfishExtensions.DTO;
using CatfishWebExtensions.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Services
{
    internal class CatfishSignInManager : ICatfishSignInManager
    {
        private string _authUrlRoot;
        private readonly string _passwordSalt;
        private readonly string _tenantName;

        private readonly IConfiguration _configuration;
        private readonly ISecurity _security;
        private readonly IJwtProcessor _jwtProcessor;
        private readonly ICatfishWebClient _catfishWebClient;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthApiProxy _authProxy;

        public CatfishSignInManager(IConfiguration configuration, ISecurity security, IJwtProcessor jwtProcessor, ICatfishWebClient catfishWebClient, UserManager<User> userManager, RoleManager<Role> roleManager, IAuthApiProxy authApiProxy)
        {
            _configuration = configuration;
            _security = security;
            _jwtProcessor = jwtProcessor;
            _catfishWebClient = catfishWebClient;
            _userManager = userManager;
            _roleManager = roleManager;
            _authProxy = authApiProxy;

            _authUrlRoot = _configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
            _passwordSalt = _configuration.GetSection("SiteConfig:LocalAccountPasswordSalt").Value;
            _tenantName = _configuration.GetSection("TenancyConfig:Name").Value;
        }

        public async Task<bool> SignOut(HttpContext context)
        {
            try
            {
                await _security.SignOut(context);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task AuthorizeSuccessfulExternalLogin(LoginResult externalLoginResult, HttpContext httpContext)
        {
            var membership = await _authProxy.GetMembership(externalLoginResult.Email);
            if (membership == null && _configuration.GetSection("SiteConfig:AllowGuestLogin").Get<bool>())
            {
                //TODO:
                //Creating a guest login account
                string guestRole = _configuration.GetSection("SiteConfig:GuestRole").Value;
                string tenantName = _configuration.GetSection("SiteConfig:Tenant").Value;
            }

            //Since this method is called when an external login is successful, we should create a local account
            //if it doesn' exists
            var user = await GetOrCreateLocalUser(membership);
            if (user == null)
                throw new CatfishException($"Unable to get or create local user {externalLoginResult?.Email}");

            //Sign-in with the local user
            var password = GetPasswordForLocalAccount(membership.User.Email);
            var status = await _security.SignIn(httpContext, user.UserName, password);

        }

        private async Task<User> GetOrCreateLocalUser(UserMembership membership)
        {

            if (string.IsNullOrEmpty(membership?.User?.UserName))
                return null;

            try
            {
                //Get the user identified in the login result
                User user = await _userManager.FindByNameAsync(membership.User.UserName);

                if (user == null)
                {
                    //Creating a profile
                    user = new User();
                    user.UserName = membership.User.UserName;
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.Email = membership.User.Email;
                    user.EmailConfirmed = true;
                    var password = GetPasswordForLocalAccount(membership.User.Email);
                    await _userManager.CreateAsync(user, password);
                }

                if(user != null)
                {
                    //Combining the membership's global roles and the currrent groups membership roles
                    //into the expected local roles. 
                    var expectedLocalRoles = new List<string>(membership.User.SystemRoles);
                    var tenancyRoles = membership.Tenancy?.FirstOrDefault(t => t.Name == _tenantName)?.Roles.Select(r => r.Name);
                    if (tenancyRoles?.Any() == true)
                        expectedLocalRoles.AddRange(tenancyRoles);


                    //Making sure the user's global roles and membership are aligned with the local roles
                    var localRoles = await _userManager.GetRolesAsync(user);
                    foreach(var role in localRoles)
                    {
                        if(!expectedLocalRoles.Contains(role))
                            await _userManager.RemoveFromRoleAsync(user, role);
                    }
                    foreach(var role in expectedLocalRoles)
                    {
                        if(!localRoles.Contains(role))
                            await _userManager.AddToRoleAsync(user, role);
                    }
               }

                return user;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetPasswordForLocalAccount(string userEmail)
            => CryptographyHelper.GenerateHash(userEmail, _passwordSalt);

        private async Task<JwtSecurityToken> AuthorizeJwt(string username, string password)
        {
            var loginUrl = $"{_authUrlRoot}/api/users/login";
            var response = await _catfishWebClient.PostJson(loginUrl, new LoginModel { UserName = username, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var jwt = await response.Content.ReadAsStringAsync();

                var jwtSecurityToken = _jwtProcessor.ReadToken(jwt);
                return jwtSecurityToken;
            }
            else
                return null;
        }

        private async Task EnsureLocalUserExists(JwtSecurityToken token, string localAccountPassword)
        {
            string email = token.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            string userdataStr = token.Claims.FirstOrDefault(c => c.Type == "userdata")?.Value;
            string membershipStr = token.Claims.FirstOrDefault(c => c.Type == "membership")?.Value;
            var roles = token.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();

            var localUser = await _userManager.FindByNameAsync(email);
            if (localUser == null)
            {
                //Creating a profile
                localUser = new User();
                localUser.UserName = email;
                localUser.NormalizedUserName = localUser.UserName.ToUpper();
                localUser.Email = email;
                localUser.EmailConfirmed = true;
                var password = GetPasswordFor(email);
                await _userManager.CreateAsync(localUser, password);
            }

            var localRoles = await _userManager.GetRolesAsync(localUser);

            //Remove any roles that are not in the roles passed on through the JWT token
            foreach (var localRole in localRoles)
            {
                if (!roles.Contains(localRole))
                    await _userManager.RemoveFromRoleAsync(localUser, localRole);
            }

            //Add any new JWT roles to the local roles if they do not exist already
            foreach (var role in roles)
            {
                if (!localRoles.Contains(role))
                {
                    if (!(await _roleManager.RoleExistsAsync(role)))
                    {
                        //Create a new role in the loca system if it doesn't exist
                        await _roleManager.CreateAsync(new Role() { Id = Guid.NewGuid(), Name = role, NormalizedName = role.ToUpper() });
                    }

                    //Assign the role to the user
                    await _userManager.AddToRoleAsync(localUser, role);
                }
            }
        }

        private string GetPasswordFor(string email)
        {
            return CryptographyHelper.GenerateHash(email, _passwordSalt);
        }
    }
}
