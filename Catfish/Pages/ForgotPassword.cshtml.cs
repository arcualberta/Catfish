
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piranha;

using System.Threading.Tasks;
using System.Linq;
using System;
using Piranha.AspNetCore.Identity.Data;

namespace Catfish.Pages
{
    //[PageTypeRoute(Title = "Default", Route = "/login")]
    public class ForgotPasswordPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {  
        private readonly ISecurity _security;
        private SignInManager<Piranha.AspNetCore.Identity.Data.User> _signInManager;
        private readonly Core.Services.IEmailService _emailSrv;

        [BindProperty]
        public string Email { get; set; }

        public string ErrorMessage { get; set; }
        
       
        public ForgotPasswordPageModel(ISecurity security,  SignInManager<Piranha.AspNetCore.Identity.Data.User> signInManager, Core.Services.IEmailService emailSrv) : base()
        {
            _security = security;
            _signInManager = signInManager;
            _emailSrv = emailSrv;
        }

        
        public async Task<IActionResult> OnPostAsync()
        {

            User user = await _signInManager.UserManager.FindByEmailAsync(Email).ConfigureAwait(false);

            if(user == null)
            {
                ErrorMessage = "Sorry, can't find the user with '" + Email + "' email address.";
                return Page();
            }

            try
            {
                string passwd = GeneratePassword(16);
                string token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await _signInManager.UserManager.ResetPasswordAsync(user, token, passwd);
                if (result.Succeeded)
                {
                    //send the new password to user
                    Core.Services.Email _email = new Core.Services.Email();
                    string uri ="https://" + HttpContext.Request.Host.ToString() + "/changepassword/true";
                    _email.Body = "<p>Here is your temporary password:<br/></p><p>" + passwd + "</p><br/><p> Please reset it by clicking the link below.</p><p>" +
                                "<a href='"+ uri +"' target='_blank'>Reset my  password </a></p>";
                    _email.RecipientEmail = Email;
                    _email.Subject = "Reset Password";
                    _email.UserName = user.UserName;
                    
                    _emailSrv.SendEmail(_email);

                    return Redirect("/changepassword/true");
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;//"Encounter problems while trying to process your request, please try again later.";
            }

            return Page();
        }

        private  static string GeneratePassword(int passLength)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz@#$&ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, passLength)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

    }
}
