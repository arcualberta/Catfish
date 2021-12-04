

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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
    public class ResetPasswordPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {  
        private readonly ISecurity _security;
        private SignInManager<Piranha.AspNetCore.Identity.Data.User> _signInManager;
      
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string CurrentPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public string SuccessMeesage { get; set; }
      

        public ResetPasswordPageModel(ISecurity security,  SignInManager<Piranha.AspNetCore.Identity.Data.User> signInManager) : base()
        {
            _security = security;
            _signInManager = signInManager;
           
        }

       
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && NewPassword.Equals(ConfirmPassword))
            {
                User user = await _signInManager.UserManager.FindByEmailAsync(Email).ConfigureAwait(false);

                if (user == null)
                {
                    ErrorMessage = "Sorry, can't find the user with '" + Email + "' email address.";
                    return Page();
                }
                try
                {
                   
                    string token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _signInManager.UserManager.ResetPasswordAsync(user, token, NewPassword);
                    if (result.Succeeded)
                    {
                        SuccessMeesage = "Your password has been succesfully updated.";
                        
                    }

                }
                catch(Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return Page();
                }
                //    return new RedirectResult("/");
            }
            else
            {
                ErrorMessage = "New Password and Confirm Password does not matched.";
            }
            

            return Page();
        }

       
    }
}
