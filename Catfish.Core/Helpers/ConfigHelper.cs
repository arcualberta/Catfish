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
        public static string[] Languages
        {
            get
            {
                var langSpec = ConfigurationManager.AppSettings["Languages"];
                if (langSpec != null)
                {
                    string[] languages = langSpec.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => s.Length > 0)
                        .ToArray();
                    return languages;
                }
                else
                    return new string[1] { "en" };
            }
        }
    }
}
