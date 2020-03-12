using Microsoft.AspNetCore.Mvc;
using Piranha;

using System.Threading.Tasks;

namespace Catfish.Pages
{
    //[PageTypeRoute(Title = "Default", Route = "/login")]
    public class LoginPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly ISecurity _security;
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public LoginPageModel(ISecurity security) : base()
        {
            _security = security;
        }
       

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && await _security.SignIn(HttpContext, Username, Password))
                return new RedirectResult("/");

            return Page();
        }
        /*
         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");
    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                                          .ToList();
    if (ModelState.IsValid)
    {
        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", 
                                      new { email = Input.Email });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // If we got this far, something failed, redisplay form
    return Page();
}
 public ActionResult ExternalLogin(Piranha.Entities.User userModel)
        {
            if (ModelState.IsValid)
            {

                Piranha.Models.LoginModel loginModel = new LoginModel();
                User user = GetUserByName(userModel.Login);
                loginModel.Login = userModel.Login;
                loginModel.Password = userModel.Password;
                if (user == null)
                {
                    if (AllowGoogleLogin())
                    {
                        user = CreateNewUser(userModel);
                        using (var db = new Piranha.DataContext())
                        {
                            // Login sysuser into the current context.
                            db.LoginSys();
                            db.Users.Add(user);
                            if (db.SaveChanges() > 0)
                            {
                                return Login(loginModel);
                            }

                        }
                    }

                }
                else
                {
                    //log he in using his gmail account -- user existed in our system

                    return Login(loginModel);
                }

            }
            else {
                return RedirectToAction("Login");
            }
            return Redirect("~/");
        }
      
         */
    }
}
