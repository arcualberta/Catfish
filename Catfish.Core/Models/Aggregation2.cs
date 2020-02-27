using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public abstract class Aggregation2 : Entity2
    {
        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Aggregation2()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();
        }
    }
}
