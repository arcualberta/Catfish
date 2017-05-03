using System.Collections.Generic;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataSet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MetadataField> Fields { get; set; }

        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            Fields = new List<MetadataField>();
        }
    }
}
