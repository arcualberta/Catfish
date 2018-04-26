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
    [TypeLabel("Metadata Definition")]
    public class MetadataDefinition
    {
        [XmlIgnore]
        public int Id { get; set; }

        public virtual List<FormField> Fields { get; set; }

        public MetadataDefinition()
        {
            Fields = new List<FormField>();
        }

        private CFXmlModel Data;

        public MetadataDefinition(CFXmlModel data, int id)
        {
            Id = id;
            Data = data;
        }

    }
}
