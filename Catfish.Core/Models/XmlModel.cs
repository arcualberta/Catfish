using Catfish.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Table("Catfish_XmlModels")]
    public class XmlModel
    {
        public enum eGuidOption { Ignore, Ensure, Regenerate }

        [Key]
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }

        public DateTime Created
        {
            get => DateTime.Parse(Data.Attribute("created").Value);
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
            Initialize(eGuidOption.Ignore);
        }

        public XmlModel(string tagName)
        {
            Data = new XElement(tagName);
            Initialize(eGuidOption.Ignore);
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
                Data.SetAttributeValue("id", Guid.NewGuid());
        }

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
