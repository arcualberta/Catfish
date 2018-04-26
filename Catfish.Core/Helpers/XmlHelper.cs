using Catfish.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Configuration;

namespace Catfish.Core.Helpers
{
    public static class XmlHelper
    {
        public static IXmlNamespaceResolver NamespaceManager { get; private set; }

        private static IEnumerable<XElement> GetChildTextElements(XElement element, string childTagName)
        {
            var xpath = "./" + childTagName + "/text";
            return ((IEnumerable)element.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();
        }

        public static IEnumerable<CFTextValue> GetTextValues(XElement element, bool forceAllLanguages = false, bool excludeBlanks = false)
        {
            List<CFTextValue> ret = new List<CFTextValue>();
            List<string> languageCodes = ConfigHelper.LanguagesCodes;

            var children = element.Elements("text");
            foreach (XElement ele in children)
            {
                if (excludeBlanks && string.IsNullOrEmpty(ele.Value))
                    continue;

                XAttribute att = ele.Attribute(XNamespace.Xml + "lang");
                string lang = att == null ? "" : att.Value;
                if (languageCodes.Contains(lang))
                {
                    CFTextValue txt = new CFTextValue(lang, ConfigHelper.GetLanguageLabel(lang), ele.Value);
                    ret.Add(txt);
                }
            }

            if (forceAllLanguages)
            {
                foreach (var lang in ConfigHelper.Languages)
                {
                    if (!ret.Where(t => t.LanguageCode == lang.TwoLetterISOLanguageName).Any())
                        ret.Add(new CFTextValue(lang.TwoLetterISOLanguageName, lang.NativeName, ""));
                }
            }

            return ret;
        }

        public static string GetLanguage(XElement element)
        {
            XAttribute att = element.Attribute(XNamespace.Xml + "lang");
            return att == null ? null : att.Value;
        }

        public static bool GetAttribute(XElement element, string attName, bool defaultValue)
        {
            XAttribute att = element.Attribute(attName);
            return att == null || string.IsNullOrEmpty(att.Value) ? defaultValue : bool.Parse(att.Value);
        }

        public static string GetAttribute(XElement element, string attName, string defaultValue)
        {
            XAttribute att = element.Attribute(attName);
            return att == null || string.IsNullOrEmpty(att.Value) ? defaultValue : att.Value;
        }

        public static int GetAttribute(XElement element, string attName, int defaultValue)
        {
            XAttribute att = element.Attribute(attName);
            return att == null || string.IsNullOrEmpty(att.Value) ? defaultValue : int.Parse(att.Value);
        }

    }
}
