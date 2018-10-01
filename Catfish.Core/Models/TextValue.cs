using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    class AllowHtmlTextValueAttribute : Attribute, IMetadataAware
    {
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            FieldInfo field = metadata.ContainerType.GetField("AllowHtml", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Static);
            metadata.RequestValidationEnabled = field == null ? false : (bool)field.GetValue(null);
        }
    }

    public class HtmlTextValue : TextValue
    {
        public new const bool AllowHtml = true;

        public HtmlTextValue()
        {
        }

        public HtmlTextValue(string langCode, string langLabel, string val) : base(langCode, langLabel, val)
        {
            
        }
    }
    
    public class TextValue
    {
        public const bool AllowHtml = false;
        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LanguageCode { get; set; }
        public string LanguageLabel { get; set; }

        [AllowHtmlTextValue]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }

        public TextValue()
        {
        }

        public TextValue(string langCode, string langLabel, string val)
        {
            LanguageCode = langCode;
            LanguageLabel = langLabel;
            Value = val;
        }

        public TextValue(XElement txtElement)
        {
            XAttribute att = txtElement.Attribute(XNamespace.Xml + "lang");
            LanguageCode = att == null ? "" : att.Value;
            Value = txtElement.Value;
        }
    }
}
