
using System;
using Microsoft.Extensions.Configuration;

namespace Catfish.Helper
{
    public interface ICatfishAppConfiguration
    {
        bool IsAllowGoogleLogin();
        string GetGoogleClientId();
        string GetDefaultUserRole();
        
    }

    public class ReadAppConfiguration : ICatfishAppConfiguration
    {
        private readonly IConfiguration _configuration;
       public ReadAppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDefaultUserRole()
        {
            return _configuration["GoogleExternalLogin:DefaultUserRole"];
        }

        public string GetGoogleClientId()
        {
            return _configuration["GoogleExternalLogin:ClientId"];
        }

        public bool IsAllowGoogleLogin()
        {
            bool allowGoogleLogin = false;
            allowGoogleLogin = Convert.ToBoolean(_configuration["GoogleExternalLogin:AllowGoogleLogin"]);

            return allowGoogleLogin;
        }
    }
}
