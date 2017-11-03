using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class TextValue
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Language { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }

        public TextValue()
        {
        }

        public TextValue(string lang, string val)
        {
            Language = lang;
            Value = val;
        }

        public TextValue(XElement txtElement)
        {
            XAttribute att = txtElement.Attribute(XNamespace.Xml + "lang");
            Language = att == null ? "" : att.Value;
            Value = txtElement.Value;
        }
    }
}
