using Catfish.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class XmlModel
    {
        [JsonIgnore]
        public XElement Data { get; protected set; }

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
            Initialize();
        }

        public XmlModel(string tagName)
        {
            Data = new XElement(tagName);
            Initialize();
        }

        public string ModelType
        {
            get => Data.Attribute("model-type") == null ? null : Data.Attribute("model-type").Value;
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(ModelType))
                Data.SetAttributeValue("model-type", GetType().AssemblyQualifiedName);
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
    }
}
