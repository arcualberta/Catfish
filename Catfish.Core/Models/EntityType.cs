using System.Collections.Generic;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

namespace Catfish.Core.Models
{
    [TypeLabel("Entity Type")]
    public class EntityType
    {
        public enum eTarget { None = 0, Collections, Items, Files, Forms };

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public eTarget TargetType { get; set; }

        [JsonIgnore] //Ignore this in JSON serialization to avoid stuck in a continuous loop
        public virtual ICollection<MetadataSet> MetadataSets { get; set; }

        [JsonIgnore] //Ignore this in JSON serialization to avoid stuck in a continuous loop
        public virtual ICollection<EntityTypeAttributeMapping> AttributeMappings { get; set; }

        [NotMapped]
        public bool HasAssociations { get { return false; } }

        public EntityType()
        {
            MetadataSets = new List<MetadataSet>();
            AttributeMappings = new List<EntityTypeAttributeMapping>();
        }

        public EntityTypeAttributeMapping GetNameMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Name Mapping").FirstOrDefault();
        }

        public EntityTypeAttributeMapping GetDescriptionMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Description Mapping").FirstOrDefault();
        }

    }
}
