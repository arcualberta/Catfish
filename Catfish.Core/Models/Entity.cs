using System;
using System.Collections;
using System.Collections.Generic;
using Catfish.Core.Models.Metadata;
using System.Web.Script.Serialization;
using Catfish.Core.Models.Attributes;

namespace Catfish.Core.Models
{
    public class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public int? EntityTypeId { get; set; }
        public EntityType EntityType { get; set; }

        ////public virtual ICollection<FieldValue> Metadata { get; set; }

        public Entity()
        {
            Created = DateTime.Now;
            ////Metadata = new List<FieldValue>();
        }
    }
}