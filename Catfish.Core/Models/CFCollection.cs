using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    public class CFCollection : CFAggregation
    {
        public override string GetTagName() { return "collection"; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual IEnumerable<CFAggregation> ChildCollections
        {
            get
            {
                return ChildMembers.Where(c => typeof(CFCollection).IsAssignableFrom(c.GetType()));
            }
        }
    }
}