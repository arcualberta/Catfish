using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public abstract class Entity2 : XmlModel
    {
        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Entity2()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();
        }
    }
}
