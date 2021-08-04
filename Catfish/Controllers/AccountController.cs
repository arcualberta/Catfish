using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.ViewModels;
using Piranha.AspNetCore.Identity.Data;
using System.Security.Claims;
using Piranha.AspNetCore.Identity.Models;
using Catfish.Helper;
using Piranha;

namespace Catfish.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        
        private readonly ICatfishAppConfiguration _catfishConfig;
        private readonly ISecurity _security;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, Piranha.AspNetCore.Identity.IDb db, ICatfishAppConfiguration catfishConfig, ISecurity security, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
           
            _signInManager = signInManager;
            _db = db;
            _userManager = userManager;
            _catfishConfig = catfishConfig;
            _security = security;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult Login(CatfishUserModel)
        //{

        //}
        //public IActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
        //}

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var q = _httpContextAccessor.HttpContext.Request.Query;
            string ret = q.Keys.Contains("ret") ? q["ret"].ToString() : "/";

            returnUrl = returnUrl ?? Url.Content(ret);

            CatfishUserViewModel loginViewModel = new CatfishUserViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            //MR: Aug 3 2021 -- check if page access is limit to certain domain only and check if iser try to login is belong to the allowed-domain
            //string AllowDomains = _catfishConfig.GetAllowDomain();
            if (!IsAllowUser(info.Principal.FindFirstValue(ClaimTypes.Email)))
            {
                return LocalRedirect(returnUrl);
            }


            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            else
            {
                // If there is no record in AspNetUserLogins table, the user may not have
                // a local account
                // Get the email claim value
                CatfishUserViewModel userModel = new CatfishUserViewModel();
                userModel.Login = info.Principal.FindFirstValue(ClaimTypes.Email);
                userModel.Firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                userModel.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                userModel.Surname = info.Principal.FindFirstValue(ClaimTypes.Surname);
                User _user = new Piranha.AspNetCore.Identity.Data.User();

                UserEditModel userEditModel = new UserEditModel();
                userEditModel.User = _user;
                userEditModel.Password = new string(userModel.Email.Reverse().ToArray());
                userEditModel.PasswordConfirm = new string(userModel.Email.Reverse().ToArray());
                // _user.

                if (await _userManager.FindByNameAsync(userModel.Login) == null)
                {
                    await _userManager.SetUserNameAsync(_user, userModel.Email);
                    await _userManager.SetEmailAsync(_user, userModel.Login);
                    await _userManager.CreateAsync(_user, userEditModel.Password);
                    string roleName = _catfishConfig.GetDefaultUserRole();
                    Role role = _db.Roles.Where(r => r.Name == roleName).FirstOrDefault();

                    userEditModel.Roles.Add(role);
                    var result = userEditModel.Save(_userManager);

                    if (result.Result.Succeeded)
                    {

                        //login user to the system
                        //if (await _security.SignIn(HttpContext, userModel.Login, userModel.Password))
                        //    return new RedirectResult("/");
                        if (await TryLogin(userModel.Login, new string(userModel.Email.Reverse().ToArray())))
                            return new RedirectResult(returnUrl);
                    }

                }
                else
                {

                    //this user already in the system -- log him/her in

                    // previously user login with google will be loginwith his/her profile Id 
                    //this one is for backward compatibility in case there're users has been signed with google account previously
                    _user = await _userManager.FindByNameAsync(userModel.Login).ConfigureAwait(false);
                    
                    if(await TryLogin(userModel.Login, new string(userModel.Email.Reverse().ToArray()), _user))
                        return new RedirectResult(returnUrl);
                }
            }
            return LocalRedirect(returnUrl);
        }

        private bool IsAllowUser(string login)
        {
            bool domainUser = false;
            string[] allowedDomains = _catfishConfig.GetAccessRestrictionAllowedDomains();
            if (allowedDomains != null && allowedDomains.Length > 0)
            {   
                    foreach (string d in allowedDomains)
                    {
                        if (login.Contains(d))
                        {
                            domainUser = true;
                            break;
                        }
                    }  
            }
            if (allowedDomains.Contains("*") || allowedDomains == null) //if allow domain is null that mean no restriction
                domainUser = true;

            return domainUser;
        }
        private async Task<bool> TryLogin(string userName, string password, User user=null)
        {
            bool bOk = false;
            try
            {
                if (await _security.SignIn(HttpContext, userName, password))
                    bOk = true;
                
            }catch(Exception ex)
            {
                if(user == null) { 
                   user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (resetPassResult.Succeeded)
                {
                    if (await _security.SignIn(HttpContext, userName, password))
                        bOk = true;
                }
            }
            return bOk;
        }
    }
}
