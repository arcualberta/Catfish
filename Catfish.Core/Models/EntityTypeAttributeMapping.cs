using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
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
    }
}
