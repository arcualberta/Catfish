using Catfish.Core.Models.Metadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catfish.Core.Models
{
    public class Item : Aggregation
    {
        public virtual ICollection<Aggregation> ParentRelations { get; set; }

        public Item()
            : base()
        {
            ParentRelations = new List<Aggregation>();
        }

        [NotMapped]
        public IEnumerable<MetadataSet> MetadataSets
        {
            get
            {
                return GetChildModels("metadata-sets/metadata-set", Data).Select(m => m as MetadataSet);
            }
        }
    }
}