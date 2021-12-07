using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Catfish.Pages
{
    //[PageTypeRoute(Title = "Default", Route = "/login")]
    public class LoginPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {  
        private readonly ISecurity _security;
        private SignInManager<Piranha.AspNetCore.Identity.Data.User> _signInManager;
      
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
       // private string returnUrl;

        public string ErrorMessage { get; set; }

        public string GetReturnUrl()
        {
            return ReturnUrl;
        }

        public void SetReturnUrl(string value)
        {
            ReturnUrl = value;
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public LoginPageModel(ISecurity security,  SignInManager<Piranha.AspNetCore.Identity.Data.User> signInManager) : base()
        {
            _security = security;
            _signInManager = signInManager;
           
        }

        /// <summary>
        /// Handle local sign in from public interface (front end)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string ret)
        {
            try
            {
                if (ModelState.IsValid && await _security.SignIn(HttpContext, Username, Password))
                    return new RedirectResult(string.IsNullOrEmpty(ret) ? "/" : ret);
                else
                    ErrorMessage = "Login Failed!";

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            ErrorMessage = "";
            this.SetReturnUrl(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }
       /// <summary>
       /// For handling Google external login
       /// </summary>
       /// <param name="provider">Google</param>
       /// <param name="returnUrl"></param>
       /// <returns></returns>
        public async Task<IActionResult> OnPostExternalLoginAsync(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(provider))
                provider = "Google";

            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", returnUrl);

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl.ToString());

            return new ChallengeResult(provider, properties);//Page();
        }


       

    }
}
