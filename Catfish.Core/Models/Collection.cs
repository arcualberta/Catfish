using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catfish.Core.Models
{
    public class Collection : CFAggregation
    {
        public override string GetTagName() { return "collection"; }

        [NotMapped]
        public virtual IEnumerable<CFAggregation> ChildCollections
        {
            get
            {
                return ChildMembers.Where(c => typeof(Collection).IsAssignableFrom(c.GetType()));
            }
        }
    }
}