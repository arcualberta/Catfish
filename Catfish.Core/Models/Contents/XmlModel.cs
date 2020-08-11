using Catfish.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class XmlModel
    {
        public enum eGuidOption { Ignore, Ensure, Regenerate }

        [NotMapped]
        [JsonIgnore]
        public XElement Data { get; protected set; }

        public string CssClass
        {
            get => GetAttribute("css-class", null);
            set => SetAttribute("css-class", value);
        }

        public static XmlModel InstantiateContentModel(XElement data)
        {
            string typeString = data.Attribute("model-type").Value;
            var type = Type.GetType(typeString);
            XmlModel model = Activator.CreateInstance(type, data) as XmlModel;
            return model;
        }

        public XmlModel(XElement data)
        {
            Data = data;
            Initialize(eGuidOption.Ensure);
        }

        public XmlModel(string tagName)
        {
            Data = new XElement(tagName);
            Initialize(eGuidOption.Ensure);
        }

        public string ModelType
        {
            get => Data.Attribute("model-type") == null ? null : Data.Attribute("model-type").Value;
        }

        public virtual void Initialize(eGuidOption guidOption)
        {
            if (string.IsNullOrEmpty(ModelType))
                Data.SetAttributeValue("model-type", GetType().AssemblyQualifiedName);

            if (guidOption == eGuidOption.Regenerate || guidOption == eGuidOption.Ensure && Data.Attribute("id") == null)
                Data.SetAttributeValue("id", Guid.NewGuid());
        }

        public void ReplaceOrInsert(XElement replacement)
        {
            if (Data.Element(replacement.Name) != null)
                Data.Element(replacement.Name).Remove();

            Data.Add(replacement);
        }

        ////protected string Lang(string lang)
        ////{
        ////    if (!string.IsNullOrEmpty(lang))
        ////        return lang;

        ////    if (System.Threading.Thread.CurrentThread.CurrentCulture != null && !string.IsNullOrEmpty(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName))
        ////        return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

        ////    return ConfigHelper.Languages[0].TwoLetterISOLanguageName;
        ////}

        ////private XmlNamespaceManager mXmlNamespaceManager;

        ////protected XmlNamespaceManager GetNamespaceManager()
        ////{
        ////    if (mXmlNamespaceManager == null)
        ////    {
        ////        mXmlNamespaceManager = new XmlNamespaceManager(new NameTable());
        ////        mXmlNamespaceManager.AddNamespace("xml", XNamespace.Xml.NamespaceName);
        ////    }

        ////    return mXmlNamespaceManager;
        ////}

        public XElement GetElement(string tagName, bool createIfNotExist)
        {
            return XmlHelper.GetElement(Data, tagName, createIfNotExist);
        }

        public XmlModel Clone()
        {
            var type = Type.GetType(ModelType);
            XmlModel model = Activator.CreateInstance(type) as XmlModel;
            model.Data = new XElement(Data);
            return model;
        }

        public bool GetAttribute(string attName, bool defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : bool.Parse(att.Value);
        }
        public void SetAttribute(string attName, bool attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

        public int GetAttribute(string attName, int defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : int.Parse(att.Value);
        }
        public void SetAttribute(string attName, int attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

        public string GetAttribute(string attName, string defaultValue)
        {
            var att = Data.Attribute(attName);
            return att == null ? defaultValue : att.Value;
        }
        public void SetAttribute(string attName, string attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

        public Guid GetAttribute(string attName, Guid defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : Guid.Parse(att.Value);
        }
        public void SetAttribute(string attName, Guid attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

    }
}
