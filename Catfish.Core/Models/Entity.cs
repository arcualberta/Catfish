using System;
using System.Collections;
using System.Collections.Generic;
using Catfish.Core.Models.Metadata;
using System.Web.Script.Serialization;
using Catfish.Core.Models.Attributes;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class Entity : XmlModel
    {
        public int? EntityTypeId { get; set; }
        public EntityType EntityType { get; set; }

        public Entity()
        {
            Data.Add(new XElement("metadata-sets"));
        }
    }
}