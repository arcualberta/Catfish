
namespace CatfishWebExtensions.Services
{
    public class CatfishUserManager : ICatfishUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Piranha.AspNetCore.Identity.IDb _db;

        public CatfishUserManager(UserManager<User> userManager, SignInManager<User> signInManager, Piranha.AspNetCore.Identity.IDb db)
        {
            _signInManager = signInManager;
            _db = db;
            _userManager = userManager;
        }

        public async Task<User> ProcessExternalLogin(LoginResult externalLoginResult)
        {

            if (!externalLoginResult.Success || string.IsNullOrEmpty(externalLoginResult.Email))
                return null;
            try
            {
                //Get the user identified in the login result
                var user = await _userManager.FindByNameAsync(externalLoginResult.Email);

                if(user == null)
                {
                    //Creating a profile
                }

                return user;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
