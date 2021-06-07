using Microsoft.AspNetCore.Http;
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
    }
}
