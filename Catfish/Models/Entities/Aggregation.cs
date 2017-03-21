using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.Entities
{
    public class Aggregation : DigitalEntity
    {
        public virtual ICollection<Aggregation> ParentMembers { get; set; }
        public virtual ICollection<Aggregation> ChildMembers { get; set; }

        public virtual ICollection<DigitalObject> ChildRelations { get; set; }

    }
}