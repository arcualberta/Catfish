using System;
using System.Collections;
using System.Collections.Generic;
using Catfish.Core.Models.Metadata;
using System.Web.Script.Serialization;
using Catfish.Core.Models.Attributes;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


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


        [NotMapped]
        public List<MetadataSet> MetadataSets
        {
            get
            {
                return GetChildModels("metadata-sets/metadata-set", Data).Select(c => c as MetadataSet).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("metadata-sets/metadata-set", Data);

                foreach (MetadataSet ms in value)
                    InsertChildElement("./metadata-sets", ms.Data);
            }
        }

    }
}