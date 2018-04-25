using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catfish.Core.Models
{
    public class CFCollection : CFAggregation
    {
        public override string GetTagName() { return "collection"; }

        [NotMapped]
        public virtual IEnumerable<CFAggregation> ChildCollections
        {
            get
            {
                return ChildMembers.Where(c => typeof(CFCollection).IsAssignableFrom(c.GetType()));
            }
        }
    }
}