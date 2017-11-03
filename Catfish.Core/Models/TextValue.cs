using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
