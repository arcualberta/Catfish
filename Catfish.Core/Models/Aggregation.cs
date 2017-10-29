using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catfish.Core.Models
{
    public class Aggregation : Entity
    {
        public virtual ICollection<Aggregation> ParentMembers { get; set; }
        public virtual ICollection<Aggregation> ChildMembers { get; set; }

        public virtual ICollection<Item> ChildRelations { get; set; }

        [NotMapped]
        public bool HasAssociations { get { return ParentMembers.Count > 0 || ChildMembers.Count > 0 || ChildRelations.Count > 0; } }


        public Aggregation()
        {
            ParentMembers = new List<Aggregation>();
            ChildMembers = new List<Aggregation>();
            ChildRelations = new List<Item>();
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AppendChild(Aggregation child)
        {
            this.ChildMembers.Add(child);
            child.ParentMembers.Add(this);
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AppendParent(Aggregation parent)
        {
            parent.ChildMembers.Add(this);
            this.ParentMembers.Add(parent);
        }

        public virtual IEnumerable<Aggregation> ChildItems
        {
            get
            {
                return ChildMembers.Where(c => typeof(Item).IsAssignableFrom(c.GetType()));
            }
        }

    }
}