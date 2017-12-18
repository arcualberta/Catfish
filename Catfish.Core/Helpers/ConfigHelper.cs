using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Core.Models.Forms;
using System.IO;

namespace Catfish.Core.Helpers
{
    public static class ConfigHelper
    {
        public static List<string> GetSettingArray(string key, char seperator)
        {
            var val = ConfigurationManager.AppSettings[key];
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
                    mLanguagesCodes = GetSettingArray("LanguageCodes", Attachment.FileGuidSeparator);

                    if (mLanguagesCodes.Count == 0)
                        mLanguagesCodes = new List<string>() { "en" };
                }
                return mLanguagesCodes;
            }
        }

        public static string GetLanguageLabel(string languageCode)
        {
            string label = Languages.Where(c => c.TwoLetterISOLanguageName == languageCode).Select(c => c.NativeName).FirstOrDefault();
            return string.IsNullOrEmpty(label) ? languageCode : label;
        }

        public static string UploadRoot { get { return ConfigurationManager.AppSettings["UploadRoot"]; } }

        public static string DataRoot { get { return Path.Combine(UploadRoot, "Data"); } }
    }
}
