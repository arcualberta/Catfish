﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Catfish.Core.Models
{
    public class Collection : Aggregation
    {
        [NotMapped]
        public string Name { get { return GetName(); } }

        //[DataType(DataType.MultilineText)]
        //public string Description { get; set; }

        public override string GetTagName() { return "collection"; }

        [NotMapped]
        public virtual IEnumerable<Aggregation> ChildCollections 
        {
            get
            {
                return ChildMembers.Where(c => typeof(Collection).IsAssignableFrom(c.GetType()));
            }
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