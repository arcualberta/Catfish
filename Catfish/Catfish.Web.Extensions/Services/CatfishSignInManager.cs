using CatfishExtensions.DTO;
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
        private readonly IConfiguration _configuration;
        private readonly ISecurity _security;
        private readonly IJwtProcessor _jwtProcessor;
        private readonly ICatfishWebClient _catfishWebClient;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private string _authUrlRoot;
        private readonly string _passwordSalt;

        public CatfishSignInManager(IConfiguration configuration, ISecurity security, IJwtProcessor jwtProcessor, ICatfishWebClient catfishWebClient, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _configuration = configuration;
            _security = security;
            _jwtProcessor = jwtProcessor;
            _catfishWebClient = catfishWebClient;
            _userManager = userManager;
            _roleManager = roleManager;
            _authUrlRoot = _configuration.GetSection("SiteConfig:AuthMicroserviceUrl").Value?.TrimEnd('/');
            _passwordSalt = _configuration.GetSection("SiteConfig:LocalAccountPasswordSalt").Value;
        }

        public async Task<bool> SignIn(User user, HttpContext httpContext)
        {
            try
            {
                var password = GetPasswordFor(user.Email);
                var token = await AuthorizeJwt(user.UserName, password);
                if(token != null)
                {
                    await EnsureLocalUserExists(token, password);
                }

                //Sign-in the user with the local account so that the permissions are granted properly.
                var status = await _security.SignIn(httpContext, user.UserName, password);
                return status;
            }
            catch (Exception)
            {
                return false;
            }
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
