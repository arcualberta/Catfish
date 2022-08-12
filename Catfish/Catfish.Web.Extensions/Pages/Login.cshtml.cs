
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


        public async Task<IActionResult> OnPostAsync(/*[FromBody] string jwt, */string returnUrl = null)
        {
            await _security.SignOut(HttpContext);


            //var result = await _googleIdentity.GetUserLoginResult(jwt);

            //var user = await _catfishUserManager.GetUser(result);
            //if (user == null)
            //    throw new CatfishException("Unable to retrieve or create user");

            ////Obtain the list of global roles of the user
            //result.GlobalRoles = await _catfishUserManager.GetGlobalRoles(user);

            //bool signInStatus = false;
            //if (bool.TryParse(_configuration.GetSection("SiteConfig:IsWebApp").Value, out bool isWebApp) && isWebApp)
            //    signInStatus = await _security.SignIn(HttpContext, Username, Password);


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
