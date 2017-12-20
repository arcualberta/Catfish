using Catfish.Core.Helpers;
using Catfish.Core.Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Catfish.Core.Models
{
    public abstract class XmlModel
    {
        public abstract string GetTagName();

        public int Id { get; set; }

        public string Guid { get; set; }

        [NotMapped]
        public DateTime Created
        {
            get
            {
                string att = GetAttribute("created", null);
                return string.IsNullOrEmpty(att) ? new DateTime() : DateTime.Parse(att);
            }
            set
            {
                SetAttribute("created", value.ToString());
            }
        }

        [NotMapped]
        public DateTime Updated
        {
            get
            {
                string att = GetAttribute("updated", null);
                return string.IsNullOrEmpty(att) ? new DateTime() : DateTime.Parse(att);
            }
            set
            {
                SetAttribute("updated", value.ToString());
            }
        }

        [Column(TypeName = "xml")]
        public string Content
        {
            get { return mContent; }
            set {
                mContent = value;
                
                this.Data = XElement.Parse(this.Content);
            }
        }

        [NotMapped]
        private string mContent;

        [NotMapped]
        private XElement mData;

        [NotMapped]
        public virtual XElement Data
        {
            get
            {
                if (mData == null && !string.IsNullOrEmpty(Content))
                    mData = XElement.Parse(Content);
                return mData;
            }

            set
            {
                if (mData != null)
                {
                    mData.Changing -= OnUpdated; // To avoid event memory leaks
                }

                mData = value;
                mData.Changing += OnUpdated;
            }
        }

        [Obsolete]
        [NotMapped]
        public string DefaultLanguage { get; set; }

        [NotMapped]
        public string Ref
        {
            get
            {
                var att = Data.Attribute("ref");
                return att != null ? att.Value : System.Guid.NewGuid().ToString("N");
            }
            set
            {
                Data.SetAttributeValue("ref", value);
            }
        }

        public XmlModel(string defaultLang = "en")
        {
            DefaultLanguage = defaultLang;
        }

        public XmlModel()
        {
            DefaultLanguage = "en";
            Data = new XElement(GetTagName());
            Created = DateTime.Now;
            Data.SetAttributeValue("model-type", this.GetType().AssemblyQualifiedName);
            Data.SetAttributeValue("IsRequired", false);
            Guid = System.Guid.NewGuid().ToString("N");
        }

        public XElement GetWrapper(string tagName, bool createIfNotExist, bool enforceGuid)
        {
            XElement wrapper = Data.Element(tagName);

            if(wrapper == null && createIfNotExist)
                Data.Add(wrapper = new XElement(tagName));

            if (wrapper != null && enforceGuid && wrapper.Attribute("guid") == null)
                wrapper.Add(new XAttribute("guid", System.Guid.NewGuid().ToString()));

            return wrapper;
        }

        public virtual IEnumerable<TextValue> GetNames(bool forceAllLanguages)
        {
            XElement wrapper = GetWrapper("name", true, false);
            return XmlHelper.GetTextValues(wrapper, forceAllLanguages);
        }

        protected virtual void SetName(IEnumerable<TextValue> val)
        {
            XElement wrapper = Data.Element("name");
            if (wrapper != null)
            {
                //removing all text elements in the wrapper
                foreach (XElement text in wrapper.Elements().Where(e => e.Name == "text").ToList())
                    text.Remove();
            }
            else
            {
                wrapper = CreateElement("name", false);
                Data.Add(wrapper);
            }

            //inserting text elements representing languages and values specified by the input argument
            foreach (TextValue v in val)
                wrapper.Add(CreateTextElement(v.Value, v.LanguageCode));
        }

        public virtual IEnumerable<TextValue> GetDescription(bool forceAllLanguages)
        {
            XElement wrapper = GetWrapper("description", true, false);
            return XmlHelper.GetTextValues(wrapper, forceAllLanguages);
        }

        protected virtual void SetDescription(IEnumerable<TextValue> val)
        {
            XElement wrapper = Data.Element("description");
            if (wrapper != null)
            {
                //removing all text elements in the wrapper
                foreach (XElement text in wrapper.Elements().Where(e => e.Name == "text").ToList())
                    text.Remove();
            }
            else
            {
                wrapper = CreateElement("description", false);
                Data.Add(wrapper);
            }

            //inserting text elements representing languages and values specified by the input argument
            foreach (TextValue v in val)
                wrapper.Add(CreateTextElement(v.Value, v.LanguageCode));
        }

        protected void OnUpdated(object sender, XObjectChangeEventArgs e)
        {
            if (sender is XAttribute && ((XAttribute)sender).Name == "updated")
            {
                return;
            }

            Updated = DateTime.Now;
        }

        protected XElement GetImmediateChild(string tagName, bool createIfNotExist = true)
        {
            XElement data = Data.Element(tagName);

            if (data == null && createIfNotExist)
                Data.Add(data = new XElement(tagName));

            return data;
        }

        public XmlModel(XElement ele, string defaultLang = "en")
        {
            Data = ele;
            DefaultLanguage = defaultLang;
        }

        public virtual string GetName(string lang = null, bool tryReturnNoneEmpty = false)
        {
            string name = GetChildText("name", Data, Lang(lang));
            if(tryReturnNoneEmpty && string.IsNullOrEmpty(name))
            {
                name = GetNames(false).Where(tv => !string.IsNullOrEmpty(tv.Value)).Select(tv => tv.Value).FirstOrDefault();
            }
            return name;
        }
        public virtual void SetName(string val, string lang = null)
        {
            SetChildText("name", val, Data, Lang(lang));
        }

        protected XElement CreateTextElement(string value, string lang)
        {
            XElement textElemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
            textElemnt.Value = value;

            return textElemnt;
        }

        protected XElement CreateElement(string tagName, bool includeGuid)
        {
            XElement element = new XElement(tagName);
            if (includeGuid)
                element.Add(new XAttribute("guid", System.Guid.NewGuid().ToString()));

            return element;
        }


        public virtual string GetDescription(string lang = null)
        {
            return GetChildText("description", Data, Lang(lang));
        }

        public virtual void SetDescription(string val, string lang = null)
        {
            SetChildText("description", val, Data, Lang(lang));
        }

        public string GetHelp(string lang = null)
        {
            return GetChildText("help", Data, Lang(lang));
        }

        public void SetHelp(string val, string lang = null)
        {
            SetChildText("help", val, Data, Lang(lang));
        }

        public virtual void SetTextValues(IEnumerable<TextValue> values)
        {
            string[] languages = values.Select(v => v.LanguageCode).Distinct().ToArray();
            foreach(string lang in languages)
            {
                IEnumerable<string> vals = values.Where(v => v.LanguageCode == lang).Select(v => v.Value);
                SetChildText("value", vals, Data, Lang(lang));
            }
        }

        public IEnumerable<string> GetValuesxx(string lang = null)
        {
            if (Data != null)
            {
                var matches = GetChildTextElements("value", Data, Lang(lang));
                return matches.Select(m => (m.Value));
            }
            return Enumerable.Empty<string>();
        }

        public virtual IEnumerable<TextValue> GetValues(bool excludeBlanks = true)
        {
            XElement wrapper = GetWrapper("value", true, false);
            return XmlHelper.GetTextValues(wrapper, false, excludeBlanks);
        }


        public virtual void SetValues(IEnumerable<string> values, string lang = null)
        {
            SetChildText("value", values, Data, Lang(lang));
        }

        public List<XmlModel> GetChildModels(string xpath, XElement ele)
        {
            List<XmlModel> result = new List<XmlModel>();

            IEnumerable<XElement> children = GetChildElements(xpath, ele);
            foreach(XElement c in children)
            {
                XmlModel model = XmlModel.Parse(c);
                result.Add(model);
            }

            return result;
        }

        protected string Lang(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
                return lang;

            if (System.Threading.Thread.CurrentThread.CurrentCulture != null && !string.IsNullOrEmpty(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName))
                return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            return ConfigHelper.Languages[0].TwoLetterISOLanguageName;
        }

        protected string GetChildText(string childTagName, XElement ele, string lang)
        {
            var matches = GetChildTextElements(childTagName, ele, lang);
            return matches.Any() ? matches.First().Value : null;
        }

        /// <summary>
        /// Checks if there is a child element exists with the given tag name that contains a text element with xml:lang set to the 
        /// given "lang". If exists, then selects the first text element that matches the criteria and sets its content
        /// to the given value. If no such text element exists, then creates a text element, sets its content value, and inserts it 
        /// as a child of the given element.
        /// </summary>
        /// <param name="childTagName"></param>
        /// <param name="val"></param>
        /// <param name="ele"></param>
        /// <param name="lang"></param>
        protected void SetChildText(string childTagName, string val, XElement ele, string lang)
        {
            var matches = GetChildTextElements(childTagName, ele, lang);

            XElement textEelemnt = null;
            if (matches.Any())
                textEelemnt = matches.First();
            else
            {

                textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
                XElement parent = ele.Element(childTagName);
                if(parent == null)
                {
                    parent = new XElement(childTagName);
                    ele.Add(parent);
                }
                parent.Add(textEelemnt);
            }

            textEelemnt.Value = val == null ? "" : val;
        }
        /// <summary>
        /// Finds the child element specified by the given tag name and then removes all text elements with
        /// the given lang un it and adds a new set of text elements with the given values.
        /// </summary>
        /// <param name="childTagName"></param>
        /// <param name="values"></param>
        /// <param name="ele"></param>
        /// <param name="lang"></param>
        protected void SetChildText(string childTagName, IEnumerable<string> values, XElement ele, string lang)
        {
            RemoveChildTextElements(childTagName, ele, lang);
            InsertChildText(childTagName, values, ele, lang);
        }

        /// <summary>
        /// Inserts the given list of values to set of new text elements inside the specified child element.
        /// </summary>
        /// <param name="childTagName"></param>
        /// <param name="values"></param>
        /// <param name="ele"></param>
        /// <param name="lang"></param>
        protected void InsertChildText(string childTagName, IEnumerable<string> values, XElement ele, string lang)
        {
            XElement parent = ele.Elements(childTagName).FirstOrDefault();
            if(parent == null)
            {
                parent = new XElement(childTagName);
                ele.Add(parent);
            }

            foreach (string val in values)
            {
                XElement textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
                textEelemnt.Value = val == null ? "" : val;
                parent.Add(textEelemnt);
            }
        }

        /// <summary>
        /// Removes all the text elements that matches the specified language form the child element specified by the
        /// given tagName inside the given element
        /// </summary>
        /// <param name="childTagName"></param>
        /// <param name="ele"></param>
        /// <param name="lang"></param>
        protected void RemoveChildTextElements(string childTagName, XElement ele, string lang)
        {
            var matches = GetChildTextElements(childTagName, ele, lang).ToList();
            foreach (var text in matches)
                text.Remove();
        }

        protected void RemoveAllElements(string xpath, XElement ele)
        {
            var chidlren = GetChildElements(xpath, ele).ToList();
            foreach (var e in chidlren)
                e.Remove();
        }

        public void InsertChildElement(string parentXPath, XElement child)
        {
            XElement parent = GetChildElements(parentXPath, Data).FirstOrDefault();
            parent.Add(child);
        }

        protected IEnumerable<XElement> GetChildTextElements(string childTagName, XElement ele, string lang)
        {
            var xpath = string.IsNullOrEmpty(lang) 
                ? "./" + childTagName + "/text" 
                : "./" + childTagName + "/text[@xml:lang='" + lang + "']";

            return GetChildElements(xpath, ele);
        }

        protected IEnumerable<XElement> GetChildElements(string xpath, XElement ele)
        {
            return ((IEnumerable)ele.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();
        }

        public string GetAttribute(string attName, XElement data = null)
        {
            if (data == null)
                data = Data;
            XAttribute att = data.Attribute(attName);
            return att == null ? null : att.Value;
        }

        public int GetAttribute(string name, int defaultValue=0, XElement data = null)
        {
            string val = GetAttribute(name, data);
            return string.IsNullOrEmpty(val) ? defaultValue : int.Parse(val);
        }

        public bool GetAttribute(string name, bool defaultValue = false, XElement data = null)
        {
            string val = GetAttribute(name, data);
            return string.IsNullOrEmpty(val) ? defaultValue : bool.Parse(val);
        }

        public void SetAttribute(string attName, string attValue, XElement data = null)
        {
            if (data == null)
                data = Data;
            data.SetAttributeValue(attName, attValue);
        }

        public void SetAttribute(string attName, int attValue, XElement data = null)
        {
            if (data == null)
                data = Data;
            data.SetAttributeValue(attName, attValue);
        }

        public void SetAttribute(string attName, bool attValue, XElement data = null)
        {
            if (data == null)
                data = Data;
            data.SetAttributeValue(attName, attValue);
        }


        protected XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (mXmlNamespaceManager == null)
                {
                    mXmlNamespaceManager = new XmlNamespaceManager(new NameTable());
                    mXmlNamespaceManager.AddNamespace("xml", XNamespace.Xml.NamespaceName);
                }

                return mXmlNamespaceManager;
            }
        }
        private XmlNamespaceManager mXmlNamespaceManager;

        public static XmlModel Parse(XElement ele, string defaultLang = "en")
        {
            string typeString = ele.Attribute("model-type").Value;
            var type = Type.GetType(typeString);
            XmlModel model = Activator.CreateInstance(type) as XmlModel;
            model.Data = ele;
            model.DefaultLanguage = defaultLang;
            return model;
        }

        public static XmlModel Load(string uri, string defaultLang = "en")
        {
            XElement root = XElement.Load(uri);
            return Parse(root, defaultLang);
        }

        //public void Deserialize()
        //{
        //    this.Data = XElement.Parse(this.Content);
        //}

        public void Serialize()
        {
            this.Content = Data.ToString();
        }

        public virtual void UpdateValues(XmlModel src)
        {
            SetTextValues(XmlHelper.GetTextValues(src.Data));
            ////this.SetValues(src.GetValues());
        }


        #region Audit Trail
        public XElement GetAuditRoot()
        {
            XElement audit = Data.Element("audit");
            if (audit == null)
                Data.Add(audit = new XElement("audit"));
            return audit;
        }
        public IEnumerable<AuditEntry> GetAuditTrail()
        {

            return GetAuditRoot().Elements("entry").Select(e => new AuditEntry() { Data = e });
        }

        public AuditEntry AddAuditEntry(AuditEntry.eAction action, string user, string target)
        {
            AuditEntry entry = new AuditEntry() { Action = action, User = user, Target = target };
            GetAuditRoot().Add(entry.Data);
            return entry;
        }

        public string GetCreator()
        {
            string xpath = "audit/entry[@action='" + AuditEntry.eAction.Create.ToString() + "']";
            XElement ele = GetChildElements(xpath, Data).FirstOrDefault();
            return ele == null ? null : ele.Attribute("user").Value;
        }

        #endregion


    }
}
