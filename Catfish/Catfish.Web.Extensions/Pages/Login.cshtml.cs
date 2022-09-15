
namespace CatfishWebExtensions.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISecurity _security;
        private SignInManager<User> _signInManager;
        private readonly IGoogleIdentity _googleIdentity;
        private readonly ManagerLocalizer _localizer;
        ICatfishUserManager _catfishUserManager;
        IConfiguration _configuration;



        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ISecurity security, SignInManager<User> signInManager, IGoogleIdentity googleIdentity, ManagerLocalizer localizer, ICatfishUserManager catfishUserManager, IConfiguration configuration)
        {
            _security = security;
            _signInManager = signInManager;
            _googleIdentity = googleIdentity;
            _localizer = localizer;
            _catfishUserManager = catfishUserManager;
            _configuration = configuration;
        }

        public void OnGet(string returnUrl = null)
        {
            // ErrorMessage = null;
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _security.SignOut(HttpContext);

            if (!ModelState.IsValid || !await _security.SignIn(HttpContext, Username, Password))
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, _localizer.General["Username and/or password are incorrect."].Value);
                return Page();
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect($"~/manager/login/auth?returnUrl={returnUrl}");
            }
            return LocalRedirect("~/manager/login/auth");
        }
    }
}
