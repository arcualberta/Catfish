using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataSet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Rank(2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<SimpleField> Fields { get; set; }

        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            Fields = new List<SimpleField>();
        }
    }
}
