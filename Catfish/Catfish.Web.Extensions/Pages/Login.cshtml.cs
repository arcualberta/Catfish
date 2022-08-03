namespace CatfishWebExtensions.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISecurity _security;
        private SignInManager<User> _signInManager;
        private readonly IGoogleIdentity _googleIdentity;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ISecurity security, SignInManager<User> signInManager, IGoogleIdentity googleIdentity)
        {
            _security = security;
            _signInManager = signInManager;
            _googleIdentity = googleIdentity;
        }

        public void OnGet()
        {
            ErrorMessage = null;
        }
    }
}
