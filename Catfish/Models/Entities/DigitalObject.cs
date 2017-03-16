using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.Entities
{
    public class DigitalObject : Aggregation
    {
        public ICollection<Aggregation> ParentRelations { get; set; }
    }
}