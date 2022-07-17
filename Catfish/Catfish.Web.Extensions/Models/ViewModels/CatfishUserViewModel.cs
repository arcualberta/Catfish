using Microsoft.AspNetCore.Authentication;

namespace CatfishWebExtensions.Models.ViewModels
{
    public class CatfishUserViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
