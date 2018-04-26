using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class CFFileOptions : XmlModel
    {
        public static string TagName { get { return "file-options"; } }
        public override string GetTagName() { return TagName; }

        [NotMapped]
        public bool PlayOnce
        {
            get
            {
                XElement playOnceElement = Data.Element("play-once");
                if (playOnceElement != null && playOnceElement.Value == "true")
                {
                    return true;                    
                }
                return false;
            }

            set
            {
                string stringValue = value == true ? "true" : "false";
                Data.SetElementValue("play-once", stringValue);
            }
        }
    }
}
