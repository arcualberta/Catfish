using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Date")]
    public class DateField : NumberField //FormField
    {
        protected TextValue FirstValue
        {
            get
            {
                foreach(TextValue value in Values)
                {
                    if(value != null && !string.IsNullOrEmpty(value.Value))
                    {
                        return value;
                    }
                }

                return null;
            }
        }

        //[NotMapped]
        //public string Value
        //{
        //    get
        //    {
        //        XElement val = Data.Element("value");
        //        return val == null ? null : val.Value;
        //    }

        //    set
        //    {
        //        XElement val = Data.Element("value");
        //        if (val == null)
        //            Data.Add(val = new XElement("value"));
        //        val.Value = value == null ? "" : value;
        //    }
        //}
        [NotMapped]
        public string Year
        {
            get
            {
                if (!string.IsNullOrEmpty(Values[0].Value))
                {
                   
                    string year = FirstValue.Value.Substring(0, 4);
                    return year;
                }
                return null;
            }

          
        }
        [NotMapped]
        public string Month
        {
            get
            {
                if (!string.IsNullOrEmpty(Values[0].Value))
                {

                    string month = FirstValue.Value.Substring(4, 2);
                    return month;
                }
                return null;
            }
           

        }
        [NotMapped]
        public string Day
        {
            get
            {
                if (!string.IsNullOrEmpty(Values[0].Value))
                {

                    string day = FirstValue.Value.Substring(6);
                    return day;
                }
                return null;
            }
           
        }

        [NotMapped]
        public string Value //date -- full date
        {
            get
            {
                string date = Year + "-" + Month +"-"+ Day;
                return date; // Values[0].Value;
            }

            set
            {
                
                XElement val = Data.Element("value");
                if (!string.IsNullOrEmpty(val.Value))
                {
                    List<TextValue> textValues = new List<TextValue>();

                    foreach (var language in Helpers.ConfigHelper.Languages)
                    {
                        TextValue textVal = new TextValue();
                        textVal.LanguageCode = language.TwoLetterISOLanguageName;
                        textVal.LanguageLabel = language.Name;
                        textVal.Value = value.Replace("-", "");
                        textValues.Add(textVal);
                    }

                    Values = textValues;
                }
               
            }
        }
    }
}
