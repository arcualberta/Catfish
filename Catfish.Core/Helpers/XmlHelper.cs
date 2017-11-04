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

        public static IEnumerable<TextValue> GetTextValues(XElement element, bool forceAllLanguages = false)
        {
            List<TextValue> ret = new List<TextValue>();

            var children = element.Elements("text");
            foreach (XElement ele in children)
            {
                XAttribute att = ele.Attribute(XNamespace.Xml + "lang");
                string lang = att == null ? "" : att.Value;
                TextValue txt = new TextValue(lang, ele.Value);
                ret.Add(txt);
            }

            if (forceAllLanguages)
            {
                string[] languages = ConfigHelper.Languages;
                foreach (var lang in languages)
                {
                    if (!ret.Where(t => t.Language == lang).Any())
                        ret.Add(new TextValue(lang, ""));
                }
            }

            return ret;
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
