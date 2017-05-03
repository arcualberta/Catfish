using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataField
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MetadataSetId { get; set; }
        public MetadataSet MetadataSet { get; set; }
    }
}
