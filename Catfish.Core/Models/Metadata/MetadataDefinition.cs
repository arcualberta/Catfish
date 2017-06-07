using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataDefinition
    {
        [TypeLabel("String")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual List<SimpleField> Fields { get; set; }

        public MetadataDefinition()
        {
            Fields = new List<SimpleField>();
        }
    }
}
