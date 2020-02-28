using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Table("Catfish_Entities")]
    public class Entity
    {
        public int Id { get; set; }

        [Column(TypeName = "xml")]
        public string Content
        {
            get => Data?.ToString();
            set => Data = string.IsNullOrEmpty(value) ? null : XElement.Parse(value);
        }

        [NotMapped]
        public virtual XElement Data { get; set; }

        ////[Column(TypeName = "uniqueidentifier")]
        public Guid Guid
        {
            get => Guid.Parse(Data.Attribute("guid").Value);
            set => Data.SetAttributeValue("guid", value);
        }

        public DateTime Created
        {
            get => DateTime.Parse(Data.Attribute("created").Value);
            set => Data.SetAttributeValue("created", value);
        }

        public DateTime? Updated
        {
            get { try { return DateTime.Parse(Data.Attribute("updated").Value); } catch (Exception) { return null as DateTime?; } }
            set => Data.SetAttributeValue("updated", value);
        }

        public string ModelType
        {
            get => Data.Attribute("model-type").Value;
        }

        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Entity()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();
        }

        public virtual void Initialize()
        {
            if (Data == null)
                Data = new XElement("entity");
            Guid = Guid.NewGuid();
            Created = DateTime.Now;
            Data.SetAttributeValue("model-type", GetType().AssemblyQualifiedName);
        }
    }
}
