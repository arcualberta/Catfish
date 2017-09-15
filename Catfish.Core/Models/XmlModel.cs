using Catfish.Core.Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Catfish.Core.Models
{
    public class XmlModel
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        [NotMapped]
        public DateTime Created
        {
            get
            {
                string att = GetAttribute("created");
                return string.IsNullOrEmpty(att) ? new DateTime() : DateTime.Parse(att);
            }
            set
            {
                SetAttribute("created", value.ToString());
            }
        }

        [Column(TypeName = "xml")]
        public string Content
        {
            get { return mContent; }
            set { mContent = value; this.Data = XElement.Parse(this.Content); }
        }

        [NotMapped]
        private string mContent;

        [NotMapped]
        public virtual XElement Data { get; set; }
        ////{
        ////    get
        ////    {
        ////        if (mData == null)
        ////        {
        ////            if(string.IsNullOrEmpty(Content))
        ////            {
        ////                Data = new XElement(GetTagName());
        ////                Data.SetAttributeValue("model-type", this.GetType().AssemblyQualifiedName);
        ////                Data.SetAttributeValue("IsRequired", false);
        ////            }
        ////            else
        ////            {
        ////                this.Data = XElement.Parse(this.Content);
        ////            }
        ////        }
        ////        return mData;
        ////    }
        ////    set
        ////    {
        ////        mData = value;
        ////    }
        ////}


        [NotMapped]
        public string DefaultLanguage { get; set; }

        [NotMapped]
        public string Ref
        {
            get
            {
                var att = Data.Attribute("ref");
                return att != null ? att.Value : null;
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

        }

        public virtual string GetTagName() { return "catfish-model"; }

        public XmlModel(XElement ele, string defaultLang = "en")
        {
            Data = ele;
            DefaultLanguage = defaultLang;
        }

        public string GetName(string lang = null)
        {
            return GetChildText("name", Data, Lang(lang));
        }
        public void SetName(string val, string lang = null)
        {
            SetChildText("name", val, Data, Lang(lang));
        }

        public string GetDescription(string lang = null)
        {
            return GetChildText("description", Data, Lang(lang));
        }

        public void SetDescription(string val, string lang = null)
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

        public IEnumerable<string> GetValues(string lang = null)
        {
            if (Data != null)
            {
                var matches = GetChildTextElements("value", Data, Lang(lang));
                return matches.Select(m => (m.Value));
            }
            return Enumerable.Empty<string>();
        }

        public void SetValues(IEnumerable<string> values, string lang = null)
        {
            SetChildText("value", values, Data, Lang(lang));
        }

        public void SetMultipleValues(IEnumerable<string> values, string language = null)
        {
            this.ClearSelected(Data);
            this.SetSelected(values, Data, Lang(language));
        }

        private void ClearSelected(XElement data)
        {
            string xpath = "./options/option";
            List<XElement> children = this.GetChildElements(xpath, data).ToList();
            foreach (XElement child in children)
            {
                child.SetAttributeValue("selected", false);
            }
        }

        private void SetSelected(IEnumerable<string> values, XElement data, string language = null)
        {
            string xpath = "./options/option";
            List<XElement> children = this.GetChildElements(xpath, data).ToList();

            foreach (string value in values)                
            {
                bool found = false;
                foreach (XElement child in children)
                {

                    IEnumerable<XElement> texts = this.GetTextElements(child, language);
                                   
                    foreach(XElement text in texts)
                    {
                        if (value == text.Value)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    if (found)
                    {
                        child.SetAttributeValue("selected", true);
                        break;
                    }
                }

                if (!found)
                {
                    data.Add(this.CreateSelectedOption(value, language));
                }
            }
        }

        //XXX Test this method
        private XElement CreateSelectedOption(string value, string language)
        {
            XElement optionElement = new XElement("option");
            XElement textElement = new XElement("text");
            optionElement.SetAttributeValue("xml:lang", Lang(language));
            textElement.Value = value;
            optionElement.Add(textElement);
            optionElement.SetAttributeValue("selected", "true");
            return optionElement;
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
            return lang == null ? DefaultLanguage : lang;
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

            textEelemnt.Value = val;
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
                textEelemnt.Value = val;
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

        //protected XElement AddChild(XElement parent, XName elementName)
        //{
        //    XElement element = new XElement(elementName);
        //    parent.Add(element);
        //    return element;
        //}

        protected IEnumerable<XElement> GetChildTextElements(string childTagName, XElement ele, string lang)
        {
            var xpath = "./" + childTagName + "/text[@xml:lang='" + lang + "']";

            //var matches = ((IEnumerable)ele.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();
            return GetChildElements(xpath, ele);
        }

        protected IEnumerable<XElement> GetTextElements(XElement xElement, string language)
        {
            string xpath = "./text[@xml:lang='" + Lang(language) + "']";            
            return GetChildElements(xpath, xElement);
        }

        protected IEnumerable<XElement> GetChildElements(string xpath, XElement ele)
        {
            return ((IEnumerable)ele.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();
        }

        public int GetAttribute(string attName, int defaultValue)
        {
            string val = GetAttribute(attName);
            return string.IsNullOrEmpty(val) ? defaultValue : int.Parse(val);
        }

        public string GetAttribute(string attName)
        {
            XAttribute att = Data.Attribute(attName);
            return att == null ? null : att.Value;
        }

        public void SetAttribute(string attName, string attValue)
        {
            Data.SetAttributeValue(attName, attValue);
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

        public void Deserialize()
        {
            this.Data = XElement.Parse(this.Content);
        }

        public void Serialize()
        {
            this.Content = Data.ToString();
        }

        public virtual void UpdateValues(XmlModel src)
        {
            //TODO: Deal with multiple languages later
            this.SetValues(src.GetValues());
        }

    }
}
