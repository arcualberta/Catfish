

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
    public class ChangePasswordPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
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

        [BindProperty]
        public bool IsReset { get; set; }
        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }
        //[BindProperty]
        //public DateTime Expired { get; set; }
      

        public ChangePasswordPageModel(ISecurity security,  SignInManager<Piranha.AspNetCore.Identity.Data.User> signInManager) : base()
        {
            _security = security;
            _signInManager = signInManager;
           
        }

        public IActionResult OnGet(bool? reset, DateTime? expired)
        {
            IsReset = reset.HasValue && reset.Value;
            //if(IsReset)
            //    Expired = expired.Value;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && NewPassword.Equals(ConfirmPassword) )
            {
               

                User user = await _signInManager.UserManager.FindByEmailAsync(Email).ConfigureAwait(false);

                if (user == null)
                {
                    ErrorMessage = "Sorry, can't find the user with '" + Email + "' email address.";
                    return Page();
                }
                try
                {
                    //check if thr given temporary password has been expired;
                    //if (IsReset && Expired.AddHours(1) < DateTime.Now)
                    //{
                    //    ErrorMessage = "Sorry your temporary password has been expired!";
                    //    //reset the temporary password, make the password invalid
                    //    string ntoken = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                    //    Random rand = new Random();
                    //    var res = await _signInManager.UserManager.ResetPasswordAsync(user, ntoken, CurrentPassword + rand.Next(1, 100));
                       
                    //    return new RedirectResult("/");
                    //}

                    //try to login with current paswd
                    bool canLogin = await TryLogin(user).ConfigureAwait(false);
                    if (!canLogin)
                        return Page();

                    //changing the passwd
                    string token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _signInManager.UserManager.ResetPasswordAsync(user, token, NewPassword);
                    if (result.Succeeded)
                    {
                        SuccessMessage = "Your password has been succesfully updated. Please continue to log in.";
                        TempData["SuccessMessage"] = SuccessMessage;
                    }

                    return Redirect("/login");
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

       private async Task<bool> TryLogin(User user)
        {
            
            bool isPersisten = false;
            try {
                var result = await _signInManager.CheckPasswordSignInAsync(user, CurrentPassword, isPersisten).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    //user can signin with current password  and now sign out the user 
                    await _signInManager.SignOutAsync();
                    return true;
                }
                else
                {
                    ErrorMessage = "Your don't have the correct password, can't change your password.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}
