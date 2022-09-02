
namespace CatfishExtensions.Helpers
{
    public enum eLoginLinkPosition
    {
        None = 0,
        Header,
        Footer
    }
    public interface ICatfishAppConfiguration
    {
        bool IsAllowGoogleLogin();
        string GetGoogleClientId();
        string GetGoogleCalendarAPIKey();
        string GetAllowDomain();
        string GetUnauthorizedLoginMessage();
        string GetGoogleClientSecret();
        string GetGoogleServiceAccountFileName();
        string GetSiteURL();
        bool GetEnabledLocalLogin();
        string[] GetAccessRestrictionAllowedDomains();
        string GetDefaultUserRole();

        bool GetValue(string key, bool defaultValue);
        string GetValue(string key, string defaultValue);
        string[] GetValue(string key, string[] defaultValue);
        int GetValue(string key, int defaultValue);

        eLoginLinkPosition GetLoginLinkPosition();

        public class ReadAppConfiguration : ICatfishAppConfiguration
        {
            private readonly IConfiguration _configuration;
            public ReadAppConfiguration(IConfiguration configuration)
            {
                _configuration = configuration;
            }


            public bool IsAllowGoogleLogin()
            {
                return GetValue("GoogleExternalLogin:AllowGoogleLogin", false);
            }
            public string GetGoogleClientId()
            {
                return _configuration["GoogleExternalLogin:ClientId"];
            }
            public string GetGoogleCalendarAPIKey()
            {
                return _configuration["Google:GoogleCalendarApiKey"];
            }
            public string GetAllowDomain()
            {
                return _configuration["GoogleExternalLogin:AuthorizedDomains"];
            }
            public string GetUnauthorizedLoginMessage()
            {
                return _configuration["GoogleExternalLogin:AuthorizationErrorMessage"];
            }
            public string GetGoogleClientSecret()
            {
                return _configuration["GoogleExternalLogin:ClientSecret"];
            }
            public string GetGoogleServiceAccountFileName()
            {
                return _configuration["GoogleCalendar:ServiceAccountFileName"];
            }
            public bool GetEnabledLocalLogin()
            {
                return GetValue("SiteConfig:EnabledLocalLogin", false);
            }
            public string GetSiteURL()
            {
                string val = _configuration["SiteConfig:SiteURL"].TrimEnd('/');
                return string.IsNullOrEmpty(val) ? "" : val;
            }
            public string GetDefaultUserRole()
            {
                return _configuration["GoogleExternalLogin:DefaultUserRole"];
            }

            public string[] GetAccessRestrictionAllowedDomains()
            {
                var allowedD = _configuration.GetSection("SiteConfig:AccessRestriction:AllowedDomains");
                string[] _domains = allowedD.Get<string[]>();

                return _domains;
            }
            public bool GetValue(string key, bool defaultValue)
            {
                bool val;
                return bool.TryParse(_configuration[key], out val)
                    ? val
                    : defaultValue;
            }
            public string GetValue(string key, string defaultValue)
            {
                string val = _configuration[key];
                return string.IsNullOrEmpty(val) ? defaultValue : val;
            }

            public string[] GetValue(string key, string[] defaultValue)
            {
                var val = _configuration.GetSection(key).Get<string[]>();
                return val == null ? defaultValue : val;
            }

            public int GetValue(string key, int defaultValue)
            {
                string val = _configuration[key];
                return string.IsNullOrEmpty(val) ? defaultValue : int.Parse(val);
            }

            public eLoginLinkPosition GetLoginLinkPosition()
            {
                string val = _configuration.GetSection("SiteConfig:LoginLinkPosition").Value;
                if (Enum.TryParse<eLoginLinkPosition>(_configuration.GetSection("SiteConfig:LoginLinkPosition").Value, out eLoginLinkPosition pos))
                    return pos;
                else
                    return eLoginLinkPosition.Header;
            }
        }
    }
}
