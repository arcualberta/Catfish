using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class XmlModel
    {
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
