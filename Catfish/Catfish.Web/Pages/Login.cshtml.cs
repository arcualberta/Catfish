
using CatfishExtensions.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.Data;
using Piranha.Manager.LocalAuth;

namespace Catfish.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISecurity _security;
        private SignInManager<User> _signInManager;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string GetReturnUrl()
        {
            return ReturnUrl;
        }

        public void SetReturnUrl(string value)
        {
            ReturnUrl = value;
        }
        public LoginModel(ISecurity security, SignInManager<User> signInManager) : base()
        {
            _security = security;
            _signInManager = signInManager;

        }

        /// <summary>
        /// Handle local sign in from public interface (front end)
        /// </summary>
        /// <param name="ret"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string ret)
        {
            try
            {
                //We don't want someone who clicked on the login button from the change-password page
                //or forgot-passwrod page to go back to those pages.
                if (!string.IsNullOrEmpty(ret) && (ret.StartsWith("/changepassword") || ret.StartsWith("/forgotPassword")))
                    ret = "";



                if (ModelState.IsValid && await _security.SignIn(HttpContext, Username, Password))
                    return new RedirectResult(ConfigHelper.SiteUrl);
                else
                    ErrorMessage = "Login Failed!";

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        /// <summary>
        /// When someone click on the login button will this method call. Then after this method will check the wether system configuration allows
        /// external google login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            ErrorMessage = "";


            returnUrl = ConfigHelper.SiteUrl;
            this.SetReturnUrl(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page(); 
        }

        /// <summary>
        /// This action handle the external google login.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> OnPostExternalLoginAsync(string provider, string returnUrl)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (string.IsNullOrEmpty(provider))
                provider = "Google";

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/";

            //var redirectUrl = string.Format("{0}/Account/ExternalLoginCallback?returnUrl={1}", ConfigHelper.SiteUrl, returnUrl);
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", returnUrl, "https");

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl.ToString());

            return new ChallengeResult(provider, properties);
        }

    }
}
