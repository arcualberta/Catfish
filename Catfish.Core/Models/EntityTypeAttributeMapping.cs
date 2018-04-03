﻿using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    public class EntityTypeAttributeMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MetadataSetId { get; set; }
        public virtual MetadataSet MetadataSet { get; set; }
        public string FieldName { get; set; }

        public string Label { get; set; }

        [NotMapped]
        public bool Deletable { get; set; }

        public EntityTypeAttributeMapping() {
            Deletable = true;
        }
    }
}
