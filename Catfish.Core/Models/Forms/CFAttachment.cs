using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Attachment Field")]
    
    public class CFAttachment : CFFormField
    {
        //public static char FileGuidSeparator = '|';

        //public string FileGuids = "";

        //[ScriptIgnore]
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

                val.Value = value == null ? "" : value;
            }
        }

        public CFAttachment()
        {
            FileGuids = "";
        }

        public override void UpdateValues(CFXmlModel src)
        {
            XElement srcValueWrapper = src.Data.Element("value");
            if (srcValueWrapper == null)
                return;

            XElement dstValueWrapper = Data.Element("value");
            if (dstValueWrapper == null)
                Data.Add(dstValueWrapper = new XElement("value"));

            dstValueWrapper.Value = srcValueWrapper.Value;
        }
    }
}
