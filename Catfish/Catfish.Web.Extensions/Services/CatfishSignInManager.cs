﻿using CatfishExtensions.DTO;
using CatfishExtensions.Interfaces.Auth;
using CatfishWebExtensions.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IUserApiProxy _authProxy;

        public CatfishSignInManager(IConfiguration configuration, ISecurity security, IJwtProcessor jwtProcessor, ICatfishWebClient catfishWebClient, UserManager<User> userManager, RoleManager<Role> roleManager, IUserApiProxy authApiProxy)
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
            //Since this is an externally logged in user, we use an internally generated password for local and AuthApi logins
            var password = GetPasswordFor(externalLoginResult.Email);

            bool status = false;
            var jwt = await _authProxy.Login(externalLoginResult.Email, password);

            if (string.IsNullOrEmpty(jwt) && _configuration.GetSection("TenancyConfig:AllowGuestLogin").Get<bool>())
            {
                //Creating a guest login account
                status = await _authProxy.Register(new RegistrationModel()
                {
                    Username = externalLoginResult.Email,
                    Email = externalLoginResult.Email,
                    Password = password,
                    ConfirmPassword = password
                });
                if (!status)
                    return; //Registration failed. Nothing much can be done beyond this point.

                jwt = await _authProxy.Login(externalLoginResult.Email, password);
            }

            if (string.IsNullOrEmpty(jwt))
                return; //Sign-in with the Auth API was not successful. Cannot move forward with it.

            //Stores JWT token in session
            httpContext.Session.SetString("JWT", jwt);

            //Get the membership by decoding the jwt string instead of making another API call below.
            var token = _jwtProcessor.ReadToken(jwt);
            var membershipStr = token.Payload.Claims.FirstOrDefault(c => c.Type == "membership")?.Value;
            if(string.IsNullOrEmpty(membershipStr))
                return; //No membership exist at all. Cannot proceed.

            var membership = JsonConvert.DeserializeObject<UserMembership>(membershipStr);
            if (membership == null)
                return; //No membership exist at all. Cannot proceed.

            //Since this method is called when an external login is successful, we should create a local account
            //if it doesn' exists
            var user = await GetOrCreateLocalUser(membership);
            if (user == null)
                throw new CatfishException($"Unable to get or create local user {externalLoginResult?.Email}");

            //Sign-in with the local user
            status = await _security.SignIn(httpContext, user.UserName, password);
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
                        {
                            if (!(await _roleManager.RoleExistsAsync(role)))
                            {
                                await _roleManager.CreateAsync(new Role()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = role,
                                    NormalizedName = role.ToUpper(),
                                });
                            }

                            await _userManager.AddToRoleAsync(user, role);

                        }
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
