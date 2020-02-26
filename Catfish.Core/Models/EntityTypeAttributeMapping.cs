using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace Catfish.Core.Models
{
    [Table("Catfish_EntityTypeAttributeMappings")]
    [Serializable]
    public class EntityTypeAttributeMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MetadataSetId { get; set; }
        public virtual MetadataSet MetadataSet { get; set; }
        public string FieldName { get; set; }

        public string Label { get; set; }

        [Column("CFEntityType_Id")]
        public int? EntityTypeId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public EntityType EntityType { get; set; }

        [NotMapped]
        public bool Deletable { get; set; }

        public EntityTypeAttributeMapping() {
            Deletable = true;
        }

        [NotMapped]
        [IgnoreDataMember]
        public FormField Field
        {
            get
            {
                return MetadataSet.Fields.Where(f => f.GetName() == FieldName).FirstOrDefault();
            }
        }
    }
}
