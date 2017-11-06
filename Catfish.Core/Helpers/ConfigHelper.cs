using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static List<Language> mLanguages;
        public static List<Language> Languages
        {
            get
            {
                if (mLanguages == null)
                {
                    var codes = GetSettingArray("LanguageCodes", '|');
                    var langages = GetSettingArray("LanguageLabels", '|');

                    if (codes.Count != langages.Count)
                        throw new Exception("Number of language codes and language labels specified in the configuration file does not match.");

                    if (codes.Count == 0)
                        mLanguages = new List<Language>() { new Language("en", "English") };
                    else
                    {
                        mLanguages = new List<Language>();
                        for (int i = 0; i < codes.Count; ++i)
                            mLanguages.Add(new Language(codes[i], langages[i]));
                    }
                }
                return mLanguages;
            }
        }

        public static string GetLanguageLabel(string languageCode)
        {
            Language lang = Languages.Where(x => x.Code == languageCode).FirstOrDefault();
            return lang == null ? languageCode : lang.Label;
        }
    }
}
