using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("Attachment Field")]
    public class Attachment: FormField
    {
        [NotMapped]
        public List<string> FileGuids
        {
            get
            {
                XElement val = Data.Element("value");
                if (val == null)
                    return new List<string>();

                return val.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }
            set
            {
                XElement val = Data.Element("value");
                if (val == null)
                    Data.Add(val = new XElement("value"));

                val.Value = string.Join("|", value);
            }
        }

        public Attachment()
        {
            FileGuids = new List<string>();
        }
    }
}
