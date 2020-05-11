
using System;
using Microsoft.Extensions.Configuration;

namespace Catfish.Helper
{
    public interface ICatfishAppConfiguration
    {
        bool IsAllowGoogleLogin();
        string GetGoogleClientId();
        string GetDefaultUserRole();
        string GetGoogleCalendarAPIKey();
        bool DisplayCarouselThumbnails();
        string GetAllowDomain();
        string GetUnauthorizedLoginMessage();
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

        public string GetGoogleCalendarAPIKey()
        {
            return _configuration["GoogleCalendar:APIKey"];
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
        public string GetAllowDomain()
        {
            return _configuration["GoogleExternalLogin:AuthorizedDomains"];
        }
        public string GetUnauthorizedLoginMessage()
        {
            return _configuration["GoogleExternalLogin:AuthorizationErrorMessage"];
        }
        public bool DisplayCarouselThumbnails()
        {
            bool displayThumbnail = false;
            displayThumbnail = _configuration["Carousel:DisplayThumbnails"] == null ? false : Convert.ToBoolean(_configuration["Carousel:DisplayThumbnails"]);

            return displayThumbnail;
        }
    }
}
