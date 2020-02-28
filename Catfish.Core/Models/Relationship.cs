using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_Relationships")]
    public class Relationship
    {
        public string Predicate { get; set; }
        public int SubjectId { get; set; }
        public Entity Subject { get; set; }
        public int ObjctId { get; set; }
        public Entity Objct { get; set; }
    }
}
