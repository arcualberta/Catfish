using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    [Obsolete("Relation is replaced with Relationship model in Catfish 2.0")]
    public class Relation
    {
        public int Id { get; set; }
        public string Relationship { get; set; }
        public int PaentId { get; set; }
        public Aggregation Parent { get; set; }
        public int RelatedItemId { get; set; }
        public Item RelatedItem { get; set; }

    }
}
