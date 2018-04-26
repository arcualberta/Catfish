using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class CFTextValue
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LanguageCode { get; set; }
        public string LanguageLabel { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }

        public CFTextValue()
        {
        }

        public CFTextValue(string langCode, string langLabel, string val)
        {
            LanguageCode = langCode;
            LanguageLabel = langLabel;
            Value = val;
        }

        public CFTextValue(XElement txtElement)
        {
            XAttribute att = txtElement.Attribute(XNamespace.Xml + "lang");
            LanguageCode = att == null ? "" : att.Value;
            Value = txtElement.Value;
        }
    }
}
