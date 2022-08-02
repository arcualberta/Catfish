using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Services
{
    internal class CatfishSignInManager : ICatfishSignInManager
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurity _security;
        public CatfishSignInManager(IConfiguration configuration, ISecurity security)
        {
            _configuration = configuration;
            _security = security;
        }

        public async Task<bool> SignIn(User user, HttpContext httpContext)
        {
            try
            {
                var password = CryptographyHelper.GenerateHash(user.Email, _configuration.GetSection("SiteConfig:LocalAccountPasswordSalt").Value);
                return await _security.SignIn(httpContext, user.UserName, password);
            }
            catch (Exception)
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
