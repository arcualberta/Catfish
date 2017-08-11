﻿using Catfish.Core.Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
       public XElement Data { get; protected set; }

        public string DefaultLanguage { get; set; }

        public XmlModel()
        {
        }

        public XmlModel(XElement ele, string lang = "en")
        {
            Data = ele;
            DefaultLanguage = lang;
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
            var matches = GetMatches("value", Data, Lang(lang));
            return matches.Select(m => (m.FirstNode as XText).Value);
        }

        public void SetValues(IEnumerable<string> values, string lang = null)
        {
            SetChildText("value", values, Data, Lang(lang));
        }

        protected string Lang(string lang)
        {
            return lang == null ? DefaultLanguage : lang;
        }

        protected string GetChildText(string childTagName, XElement ele, string lang)
        {
            var matches = GetMatches(childTagName, ele, lang);
            return matches.Any() ? (matches.First().FirstNode as XText).Value : null;
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
            var matches = GetMatches(childTagName, ele, lang);

            XElement textEelemnt = null;
            if (matches.Any())
                textEelemnt = matches.First();
            else
            {
                textEelemnt = new XElement("text", new XAttribute(XNamespace.Xml + "lang", lang));
                ele.Add(textEelemnt);
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
            XElement parent = ele.Elements(childTagName).First();
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
            var matches = GetMatches(childTagName, ele, lang).ToList();
            foreach (var text in matches)
                text.Remove();
        }

        protected IEnumerable<XElement> GetMatches(string childTagName, XElement ele, string lang)
        {
            var xpath = "/" + childTagName + "/text[@xml:lang='" + lang + "']";

            var matches = ((IEnumerable)ele.XPathEvaluate(xpath, NamespaceManager)).Cast<XElement>();

            return matches;
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

        [Required]
        [TypeLabel("String")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [TypeLabel("String")]
        public string Description { get; set; }


        public virtual XElement ToXml()
        {
            string tagName = Name;

            if (string.IsNullOrWhiteSpace(tagName))
                tagName = GetType().ToString();

            tagName = Regex.Replace(tagName, @"\s+", string.Empty);

            try
            {
                tagName = XmlConvert.VerifyName(tagName);
            }
            catch(ArgumentNullException)
            {
                tagName = "Document";
            }
            catch(XmlException)
            {
                tagName = XmlConvert.EncodeName(tagName);
            }

            XElement ele = new XElement(tagName);

            if (Name != tagName)
                ele.SetAttributeValue("Name", string.IsNullOrEmpty(Name) ? "" : Name);

            ele.SetAttributeValue("ModelType", GetType().AssemblyQualifiedName);

            if (!string.IsNullOrWhiteSpace(Description))
                ele.Add(new XElement("Description") { Value = Description });

            return ele;
        }

        public virtual void Initialize(XElement ele)
        {
            this.Name = GetAtt(ele, "Name", ele.Name.LocalName);
            this.Description = GetChildText(ele, "Description");
        }

        public static XmlModel Parse(XElement ele)
        {
            string typeString = ele.Attribute("ModelType").Value;
            var type = Type.GetType(typeString);
            XmlModel field = Activator.CreateInstance(type) as XmlModel;
            field.Initialize(ele);
            return field;
        }

        protected static string GetAtt(XElement ele, string attName, string defaultValue = null)
        {
            var att = ele.Attribute(attName);
            return att == null ? defaultValue : att.Value;
        }
        protected static string GetChildText(XElement ele, string childTagName, string defaultValue = null)
        {
            var child = ele.Element(childTagName);
            return child == null ? defaultValue : child.Value;
        }

    }
}
