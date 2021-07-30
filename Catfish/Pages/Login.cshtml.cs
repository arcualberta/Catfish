using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System.Collections.Generic;
using System.Threading.Tasks;
using Piranha.AspNetCore.Identity.Models;
using System.Linq;
using Catfish.Models.ViewModels;
using System.Security.Claims;
using Piranha.AspNetCore.Identity.Data;
using Catfish.Helper;

namespace Catfish.Pages
{
    //[PageTypeRoute(Title = "Default", Route = "/login")]
    public class LoginPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        private readonly ISecurity _security;
        private SignInManager<Piranha.AspNetCore.Identity.Data.User> _signInManager;
        private UserManager<Piranha.AspNetCore.Identity.Data.User> _userManager;
        private readonly ICatfishAppConfiguration _catfishConfig;


        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private string returnUrl;

        public string GetReturnUrl()
        {
            return returnUrl;
        }

        public void SetReturnUrl(string value)
        {
            returnUrl = value;
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public LoginPageModel(ISecurity security, Piranha.AspNetCore.Identity.IDb db, SignInManager<Piranha.AspNetCore.Identity.Data.User> signInManager, UserManager<Piranha.AspNetCore.Identity.Data.User> userManager, ICatfishAppConfiguration catfishConfig) : base()
        {
            _security = security;
            _signInManager = signInManager;
            _userManager = userManager;
            _catfishConfig = catfishConfig;
            _db = db;

        }

        /// <summary>
        /// Handle local sign in from public interface (front end)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && await _security.SignIn(HttpContext, Username, Password))
                return new RedirectResult("/");

            return Page();
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            this.SetReturnUrl(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }
        public async Task<IActionResult> ExternalLoginCallbackAsync(string returnUrl=null, string remoteError=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            SetReturnUrl(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider:{remoteError}");
                return Page();
            }

            var userInfo = await _signInManager.GetExternalLoginInfoAsync();
            if(userInfo == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information.");
                return new RedirectResult("~/login");
            }
            //at this point login should be successfull
            // if there's limitation to certain domain -- check the user's email if it's not in allowed domain -- return to main page

            //login user
            //var email = userInfo.Principal.FindFirstValue(ClaimType.Email)
            CatfishUserViewModel userModel = new CatfishUserViewModel();
            userModel.Login = userInfo.Principal.FindFirstValue(ClaimTypes.Email);
            userModel.Firstname = userInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
            userModel.Email = userInfo.Principal.FindFirstValue(ClaimTypes.Email);
            userModel.Surname = userInfo.Principal.FindFirstValue(ClaimTypes.Surname);
            //userModel.ReturnUrl = returnUrl;

            // RedirectToPage("Catfishuser",new {userModel = userModel });
            _ = await checkUser(userModel).ConfigureAwait(false);
           
            //return LocalRedirect(returnUrl);
            return Page();
        }
        public async Task<IActionResult> OnPostExternalLoginAsync(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(provider))
                provider = "Google";

            var redirectUrl = RedirectToPage("Login","ExternalLoginCallbackAsync", new { ReturnUrl = returnUrl });//Url.Action("ExternalLoginCallbackAsync", new { ReturnUrl = returnUrl });//ExternalLoginCallbackAsync(returnUrl); //Url.Action("ExternalLoginCallback", "Account", returnUrl);



            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl.ToString());

            return new ChallengeResult(provider, properties);//Page();
        }


        public async Task<IActionResult> checkUser(CatfishUserViewModel userModel)
        {
           
            if (ModelState.IsValid)
            {
               
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
                        if (await _security.SignIn(HttpContext, userModel.Login, userModel.Password))
                            return new RedirectResult("/");
                    }

                }
                else
                {

                    //this user already in the system -- log him/her in

                    // previously user login with google will be loginwith his/her profile Id 
                    //this one is for backward compatibility in case there're users has been signed with google account previously
                    if (await _security.SignIn(HttpContext, userModel.Login, userModel.Password))
                        return new RedirectResult("/");

                    //if the login with user'd profile id failed -- try to login with user's email hash
                    if (await _security.SignIn(HttpContext, userModel.Login, new string(userModel.Email.Reverse().ToArray())))
                        return new RedirectResult("/");
                }
            }
            return Page();
        }

    }
}
