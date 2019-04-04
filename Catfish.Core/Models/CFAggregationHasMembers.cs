using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    [Table("AggregationHasMembers")]
    public class CFAggregationHasMembers
    {
        [Key, Column(Order = 0)]
        public int ParentId { get; set; }

        public virtual CFAggregation Parent { get; set; }

        [Key, Column(Order = 1)]
        public int ChildId { get; set; }

        public virtual CFAggregation Child { get; set; }

        [Key, Column(Order = 2)]
        public int Order { get; set; }
    }
}
