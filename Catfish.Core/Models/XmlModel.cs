using Catfish.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models
{
    //[Table("Catfish_XmlModels")]
    public class XmlModel
    {
        public enum eGuidOption { Ignore, Ensure, Regenerate }

        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }

        public DateTime Created
        {
            get { try { return Data.Attribute("created") != null ? DateTime.Parse(Data.Attribute("created").Value) : DateTime.MinValue; } catch (Exception) { return DateTime.MinValue; } }
            set => Data.SetAttributeValue("created", value);
        }

        public DateTime? Updated
        {
            get { try { return Data.Attribute("updated") != null ? DateTime.Parse(Data.Attribute("updated").Value) : null as DateTime?; } catch (Exception) { return null as DateTime?; } }
            set => Data.SetAttributeValue("updated", value);
        }

        [JsonIgnore]
        [Column(TypeName = "xml")]
        public string Content
        {
            get => Data?.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Data = XElement.Parse(value);
                }
            }
        }

        [JsonIgnore]
        [NotMapped]
        public XElement Data { get; protected set; }

        [NotMapped]
        public string CssClass
        {
            get => GetAttribute("css-class", null as string);
            set => SetAttribute("css-class", value);
        }

        public XmlModel Clone()
        {
            var type = Type.GetType(ModelType);
            XmlModel model = Activator.CreateInstance(type, new XElement(Data)) as XmlModel;
            return model;
        }
        public T Clone<T>() where T : XmlModel
        {
            var type = typeof(T);
            T model = Activator.CreateInstance(type, new XElement(Data)) as T;
            return model;
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
        public XmlModel()
        {

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
                SetNewGuid();
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

        public void SetNewGuid()
        {
            Data.SetAttributeValue("id", Guid.NewGuid());
        }
        public XElement GetElement(string tagName, bool createIfNotExist)
        {
            return XmlHelper.GetElement(Data, tagName, createIfNotExist);
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

        public int? GetAttribute(string attName, int? defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : int.Parse(att.Value);
        }

        public void SetAttribute(string attName, int? attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

        public long GetAttribute(string attName, long defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : long.Parse(att.Value);
        }
        public void SetAttribute(string attName, long attValue)
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

        public Guid? GetAttribute(string attName, Guid? defaultValue)
        {
            var att = Data.Attribute(attName);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? defaultValue : Guid.Parse(att.Value);
        }
        public void SetAttribute(string attName, Guid attValue)
        {
            Data.SetAttributeValue(attName, attValue);
        }

        public void SetAttribute(string attName, Guid? attValue)
        {
            if (attValue.HasValue)
                Data.SetAttributeValue(attName, attValue.Value);
            else
            {
                var att = Data.Attribute(attName);
                if (att != null)
                    att.Remove();
            }
        }


        public Guid[] GetAttribute(string attName, Guid[] defaultValue)
        {
            var att = Data.Attribute(attName);
            if (defaultValue == null)
                defaultValue = new Guid[0];

            return (att == null || string.IsNullOrEmpty(att.Value)) 
                ? defaultValue 
                : att.Value.Split(",", StringSplitOptions.RemoveEmptyEntries)
                     .Select(val => Guid.Parse(val))
                     .ToArray();
        }
        public void SetAttribute(string attName, Guid[] attValue)
        {
            if (attValue == null)
                Data.SetAttributeValue(attName, "");
            else
                Data.SetAttributeValue(attName, string.Join(",", attValue.Select(val => val.ToString())));
        }

        public string[] GetAttribute(string attName, string[] defaultValue)
        {
            var att = Data.Attribute(attName);
            if (defaultValue == null)
                defaultValue = new string[0];

            return (att == null || string.IsNullOrEmpty(att.Value))
                ? defaultValue
                : att.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        public void SetAttribute(string attName, string[] attValue)
        {
            if (attValue == null)
                Data.SetAttributeValue(attName, "");
            else
                Data.SetAttributeValue(attName, string.Join(",", attValue));
        }

        public T GetAttribute<T>(string attName, T defaultValue) where T : Enum
        {
            var att = Data.Attribute(attName);
            return att == null ? defaultValue : (T) Enum.Parse(typeof(T), att.Value) ;
        }

        public void SetAttribute<T>(string attName, T defaultValue) where T : Enum
        {
            Data.SetAttributeValue(attName, defaultValue);
        }
    }
}
