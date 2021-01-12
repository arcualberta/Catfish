﻿
using System;
using Microsoft.Extensions.Configuration;

namespace Catfish.Helper
{
    public interface ICatfishAppConfiguration
    {
        public enum ePanelLocation { None, Header, Body }

        bool IsAllowGoogleLogin();
        string GetGoogleClientId();
        string GetDefaultUserRole();
        string GetGoogleCalendarAPIKey();
        string GetSolrUrl();
        bool DisplayCarouselThumbnails();
        string GetAllowDomain();
        string GetUnauthorizedLoginMessage();
        string GetSmtpServer();
        string GetSmtpEmail();
        string GetRecipientEmail();
        string GetLogoUrl();
        bool EnableWorkflows();
        string GetWorkflowHomePageTitle();
        string GetWorkflowListPageTitle();
        string GetDefaultLanguage();
        string GetGoogleClientSecret();
        string GetGoogleServiceAccountFileName();
        bool GetEnabledLocalLogin();
        bool GetEnabledBreadcrumb();
        ePanelLocation GetDefaultSearchPanelLocation();

        bool GetValue(string key, bool defaultValue);
        string GetValue(string key, string defaultValue);
        string[] GetValue(string key, string[] defaultValue);
        int GetValue(string key, int defaultValue);

    }

    public class ReadAppConfiguration : ICatfishAppConfiguration
    {
        private readonly IConfiguration _configuration;
        public ReadAppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
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
        public string GetGoogleClientSecret()
        {
            return _configuration["GoogleExternalLogin:ClientSecret"];
        }

        public bool IsAllowGoogleLogin()
        {
            return GetValue("GoogleExternalLogin:AllowGoogleLogin", false);
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
            return GetValue("SiteConfig:ShowCarouselThumbnails", false);
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

        public string GetSolrUrl()
        {
            return _configuration["SolarConfiguration:solrItemURL"];
        }

        public string GetLogoUrl()
        {
            return _configuration["SiteConfig:LogoUrl"];
        }

        public bool EnableWorkflows()
        {
            return GetValue("Workflow:Enable", false);
        }

        public string GetWorkflowHomePageTitle()
        {
            return _configuration["Workflow:StartPageTitle"];
        }

        public string GetWorkflowListPageTitle()
        {
            return _configuration["Workflow:ListPageTitle"];
        }

        public string GetDefaultLanguage()
        {
            return GetValue("SiteConfig:DefaultLanguage", "en");
        }

        public string GetGoogleServiceAccountFileName()
        {
            return _configuration["GoogleCalendar:ServiceAccountFileName"];
        }
        public bool GetEnabledLocalLogin()
        {
            return GetValue("SiteConfig:EnabledLocalLogin", false);
        }

        public bool GetEnabledBreadcrumb()
        {
            return GetValue("SiteConfig:EnableBreadcrumb", false);
        }

        public ICatfishAppConfiguration.ePanelLocation GetDefaultSearchPanelLocation()
        {
            string configVal = GetValue("SiteConfig:SearchPanelLocation", null as string);
            return string.IsNullOrEmpty(configVal)
                ? ICatfishAppConfiguration.ePanelLocation.Header
                : (ICatfishAppConfiguration.ePanelLocation)Enum.Parse(typeof(ICatfishAppConfiguration.ePanelLocation), configVal);
        }
    }
}
