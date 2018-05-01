using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [Obsolete]
    [CFTypeLabel("Metadata Definition")]
   
    public class CFMetadataDefinition
    {
        [XmlIgnore]
        public int Id { get; set; }

        public virtual List<CFFormField> Fields { get; set; }

        public CFMetadataDefinition()
        {
            Fields = new List<CFFormField>();
        }

        private CFXmlModel Data;

        public CFMetadataDefinition(CFXmlModel data, int id)
        {
            Id = id;
            Data = data;
        }

    }
}
