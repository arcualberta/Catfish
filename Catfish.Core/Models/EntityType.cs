using System.Collections.Generic;
using Catfish.Core.Models.Metadata;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catfish.Core.Models
{
    [TypeLabel("Entity Type")]
    public class EntityType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MetadataSet> MetadataSets { get; set; }

        public virtual ICollection<EntityTypeAttributeMapping> AttributeMappings { get; set; }
        public EntityType()
        {
            MetadataSets = new List<MetadataSet>();
            AttributeMappings = new List<EntityTypeAttributeMapping>();
        }
    }
}
