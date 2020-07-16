
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
#pragma warning disable CA1055 // Uri return values should not be strings
        string GetSolrUrl();
#pragma warning restore CA1055 // Uri return values should not be strings
        bool DisplayCarouselThumbnails();
        string GetAllowDomain();
        string GetUnauthorizedLoginMessage();
        string GetSmtpServer();
        string GetSmtpEmail();
      
        string GetRecipientEmail();
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

        public string GetSmtpServer()
        {
            return _configuration["EmailServer:SmtpServer"];
        }

        public string GetSmtpEmail()
        {
            return _configuration["EmailServer:Email"];
        }

       

        public string GetRecipientEmail()
        {
             return _configuration["EmailServer:Recipient"];
        }

#pragma warning disable CA1055 // Uri return values should not be strings
        public string GetSolrUrl()
#pragma warning restore CA1055 // Uri return values should not be strings
        {
            return _configuration["SolarConfiguration:solrItemURL"];
        }
    }
}
