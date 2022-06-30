using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Catfish.Core.Helpers
{
    public static class ConfigHelper
    {

        public static IConfiguration Configuration;
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static string UploadRoot
        {
            get
            {
                var configSection = Configuration.GetSection("SiteConfig:UploadRoot");
                string path = (configSection == null || string.IsNullOrEmpty(configSection.Value))
                    ? "App_Data/uploads/"
                    : configSection.Value;

                return path;
            }
        }

        public static string SiteUrl
        {
            get
            {
                string val = Configuration.GetSection("SiteConfig:SiteUrl").Value;
                return string.IsNullOrEmpty(val) ? "" : val.TrimEnd('/');
            }
        }

        public static bool GetConfigVal(string name, bool defaultValue)
        {
            string val = Configuration.GetSection(name).Value;
            return string.IsNullOrEmpty(val) ? defaultValue : bool.Parse(val);
        }



        public static string GetUploadTempFolder(bool createIfNotExist = false)
        {
            string path = Path.Combine(UploadRoot, "temp");

            if (createIfNotExist)
                Directory.CreateDirectory(path);

            return path;
        }

        public static string GetAttachmentsFolder(bool createIfNotExist = false)
        {
            string path = Path.Combine(UploadRoot, "attachments"); ;

            if (createIfNotExist)
                Directory.CreateDirectory(path);

            return path;
        }


        //public enum eImageSize { Thumbnail = 150, Small = 256, Medium = 512, Large = 1024 } //in px

        public static char FileGuidSeparator = '|';

        public static List<string> GetSettingArray(string key, char seperator)
        {
            string val = Configuration[key];
            if (val != null)
            {
                List<string> arr = val.Split(new char[] { seperator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => s.Length > 0)
                    .ToList();
                return arr;
            }
            else
                return new List<string>();
        }

        private static List<CultureInfo> mLanguages;
        public static List<CultureInfo> Languages
        {
            get
            {
                if (mLanguages == null)
                {
                    var codes = LanguagesCodes;
                    mLanguages = new List<CultureInfo>();
                    for (int i = 0; i < codes.Count; ++i)
                        mLanguages.Add(new CultureInfo(codes[i]));
                }
                return mLanguages;
            }
        }

        private static List<string> mLanguagesCodes;
        public static List<string> LanguagesCodes
        {
            get
            {
                if (mLanguagesCodes == null)
                {
                    mLanguagesCodes = GetSettingArray("LanguageCodes", FileGuidSeparator);

                    if (mLanguagesCodes.Count == 0)
                        mLanguagesCodes = new List<string>() { "en" };
                }
                return mLanguagesCodes;
            }
        }

        public static string DefaultLanguageCode
        {
            get
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture != null && !string.IsNullOrEmpty(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName))
                    return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

                return ConfigHelper.Languages[0].TwoLetterISOLanguageName;

            }
        }

        //public static string GetLanguageLabel(string languageCode)
        //{
        //    string label = Languages.Where(c => c.TwoLetterISOLanguageName == languageCode).Select(c => c.NativeName).FirstOrDefault();
        //    return string.IsNullOrEmpty(label) ? languageCode : label;
        //}


        //public static int ThumbnailSize { get { return 150; } }
        //public static int PageSize { get { return (Configuration["PageSize"] != null) ? int.Parse(Configuration["PageSize"]) : 25; } }
    }
}
