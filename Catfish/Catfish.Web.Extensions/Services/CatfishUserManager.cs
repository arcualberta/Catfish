
namespace CatfishWebExtensions.Services
{
    public class CatfishUserManager : ICatfishUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
        private readonly ISecurity _security;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        
        public CatfishUserManager(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager, 
            SignInManager<User> signInManager, 
            Piranha.AspNetCore.Identity.IDb db, 
            IConfiguration configuration,
            ISecurity security,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
            _configuration = configuration;
            _security = security;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public Methods

        public async Task<User> GetUser(LoginResult externalLoginResult)
        {

            if (!externalLoginResult.Success || string.IsNullOrEmpty(externalLoginResult.Email))
                return null;
            try
            {
                //Get the user identified in the login result
                User user = await _userManager.FindByNameAsync(externalLoginResult.Email);
                
                if (user == null)

                {
                    //Creating a profile
                    user = new User();
                    user.UserName = externalLoginResult.Email;
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.Email = externalLoginResult.Email;
                    user.EmailConfirmed = true;
                    var password = GetPasswordHash(externalLoginResult.Email);
                    await _userManager.CreateAsync(user, password);

                    var defaultRole = await GetDefaultRole();
                    await _userManager.AddToRoleAsync(user, defaultRole.Name);
                }

                return user;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> SignIn(User user, HttpContext httpContext)
        {
            try
            {
                var password = GetPasswordHash(user.Email);
                return await _security.SignIn(httpContext, user.UserName, password);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<bool> SignOut(HttpContext context)
        {
            try
            {
                await _security.SignOut(context);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<IList<string>> GetGlobalRoles(User user)
        {
            var roels = await _userManager.GetRolesAsync(user);
            return roels;
        }

        #endregion

        #region Private Methods

        private async Task<Role> GetDefaultRole()
        {
            string defaultRoleName = _configuration.GetSection("SiteConfig:DefaultUserRole").Value;
            if (string.IsNullOrEmpty(defaultRoleName))
                defaultRoleName = "User";
            Role role = _db.Roles.FirstOrDefault(r => r.Name == defaultRoleName);
            if (role == null)
            {
                role = new Role()
                {
                    Name = defaultRoleName,
                    NormalizedName = defaultRoleName.ToUpper(),
                    Id = Guid.NewGuid()
                };
                await _roleManager.CreateAsync(role);
            }

            return role;
        }

        private string GetPasswordHash(string password)
        {
            var salt = _configuration.GetSection("SiteConfig:LocalAccountPasswordSalt").Value;
            var hash = CryptographyHelper.GenerateHash(password, salt);

            return hash;
        }

        #endregion
    }
}
