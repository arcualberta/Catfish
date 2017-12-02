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
        public string FileGuids
        {
            get
            {
                XElement val = Data.Element("value");
                return val == null ? "" : val.Value;
            }
            set
            {
                XElement val = Data.Element("value");
                if (val == null)
                    Data.Add(val = new XElement("value"));

                val.Value = value;
            }
        }

        public Attachment()
        {
            FileGuids = "";
        }
    }
}
