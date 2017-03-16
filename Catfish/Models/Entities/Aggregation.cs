using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.Entities
{
    public class Aggregation
    {
        public int Id { get; set; }

        public ICollection<Aggregation> ParentMembers { get; set; }
        public ICollection<Aggregation> ChildMembers { get; set; }

        public ICollection<DigitalObject> ChildRelations { get; set; }

    }
}