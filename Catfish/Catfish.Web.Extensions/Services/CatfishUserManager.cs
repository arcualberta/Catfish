
namespace CatfishWebExtensions.Services
{
    [Obsolete]
    public class CatfishUserManager : ICatfishUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
        private readonly CatfishExtensions.Interfaces.IGoogleIdentity _googleIdentity;
        private readonly IJwtProcessor _jwtProcessor;
      
        public CatfishUserManager(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            Piranha.AspNetCore.Identity.IDb db,
            IConfiguration configuration,
            IGoogleIdentity googleIdentity,
            IJwtProcessor jwtProcessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _configuration = configuration;
            _googleIdentity = googleIdentity;
            _jwtProcessor = jwtProcessor;
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
                    var password = CryptographyHelper.GenerateHash(externalLoginResult.Email, _configuration.GetSection("SiteConfig:LocalAccountPasswordSalt").Value);
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


        public async Task<IList<string>> GetGlobalRoles(User user)
        {
            var roels = await _userManager.GetRolesAsync(user);
            return roels;
        }
        public async Task<string> GetUserJwtLoginTokenAsync(string userName, bool isAuthenticated)
        {
            User user = await _userManager.FindByNameAsync(userName);


            LoginResult userLoginResult = new LoginResult();
            userLoginResult.Success = isAuthenticated;
            userLoginResult.Name = userName;
            userLoginResult.Email = user.Email;
            var usrRoles = await _userManager.GetRolesAsync(user);
            userLoginResult.GlobalRoles = usrRoles;

            var jwt = _jwtProcessor.GenerateJSonWebToken(userLoginResult);
            return jwt;
            
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
        #endregion
       
       
    }
}
