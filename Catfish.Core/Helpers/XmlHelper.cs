using Catfish.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Reflection;

namespace Catfish.Core.Helpers
{
    public static class XmlHelper
    {
        public static XElement GetElement(XElement parent, string tagName, bool createIfNotExist)
        {
            XElement ele = parent.Element(tagName);
            if (ele == null && createIfNotExist)
            {
                ele = new XElement(tagName);
                parent.Add(ele);
            }

            return ele;
        }

        /*
                public static IXmlNamespaceResolver NamespaceManager { get; private set; }

                private static IEnumerable<XElement> GetChildTextElements(XElement element, string childTagName)
                {
                    var xpath = "./" + childTagName + "/text";
                    return ((IEnumerable)element.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();
                }

                public static IEnumerable<TextValue> GetTextValues(XElement element, bool includeAllLanguages = false, bool excludeBlanks = false, string activeLang=null)
                {
                    return GetTextValues<TextValue>(element, includeAllLanguages, excludeBlanks, activeLang);
                }

                public static IEnumerable<T> GetTextValues<T>(XElement element, bool includeAllLanguages = false, bool excludeBlanks = false, string activeLang = null) where T : TextValue
                {
                    List<T> ret = new List<T>();
                    List<string> languageCodes = ConfigHelper.LanguagesCodes;
                    ConstructorInfo cInfo = typeof(T).GetConstructor(new[] { typeof(string), typeof(string), typeof(string) });

                    var children = element.Elements("text");
                    foreach (XElement ele in children)
                    {
                        if (excludeBlanks && string.IsNullOrEmpty(ele.Value))
                            continue;
                        XAttribute att = ele.Attribute(XNamespace.Xml + "lang");
                        string lang = att == null ? "" : att.Value.Trim();

                        if (string.IsNullOrEmpty(activeLang))
                        {

                            if (languageCodes.Contains(lang))
                            {
                                T txt = (T)cInfo.Invoke(new[] { lang, ConfigHelper.GetLanguageLabel(lang), ele.Value });
                                ret.Add(txt);
                            }
                        }
                        else
                        {

                            if (lang.Equals(activeLang))
                            {
                                T txt = (T)cInfo.Invoke(new[] { activeLang, ConfigHelper.GetLanguageLabel(activeLang), ele.Value });
                                ret.Add(txt);

                            }
                        }
                    }

                    if (includeAllLanguages)
                    {
                        foreach (var lang in ConfigHelper.Languages)
                        {
                            if (!ret.Where(t => t.LanguageCode == lang.TwoLetterISOLanguageName).Any())
                                ret.Add((T)cInfo.Invoke(new[] { lang.TwoLetterISOLanguageName, lang.NativeName, "" }));
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
        */
    }
}
