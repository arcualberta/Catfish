
namespace CatfishWebExtensions.Services
{
    public class CatfishUserManager : ICatfishUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
      
        public CatfishUserManager(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager, 
            Piranha.AspNetCore.Identity.IDb db, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _configuration = configuration;
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
