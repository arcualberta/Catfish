using System.Collections.Generic;

namespace Catfish.Core.Models
{
    public class Item : Aggregation
    {
        public virtual ICollection<Aggregation> ParentRelations { get; set; }
    }
}