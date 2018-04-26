using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Date")]
    public class DateField : FormField
    {
        [NotMapped]
        public string Value
        {
            get
            {
                XElement val = Data.Element("value");
                return val == null ? null : val.Value;
            }

            set
            {
                XElement val = Data.Element("value");
                if (val == null)
                    Data.Add(val = new XElement("value"));
                val.Value = value == null ? "" : value;
            }
        }

    }
}
