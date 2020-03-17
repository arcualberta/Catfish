using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Table("Catfish_Entities")]
    public class Entity
    {
        //public Guid Id { get; set; }
        [Key]
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }


        [Column(TypeName = "xml")]
        public string Content
        {
            get => Data?.ToString();
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    Data = XElement.Parse(value);
                    Initialize();
                }
            }
        }

        [NotMapped]
        public virtual XElement Data { get; set; }

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

        [NotMapped]
        public MultilingualElement Name { get; protected set; }
        [NotMapped]
        public MultilingualElement Description { get; protected set; }

        [NotMapped]
        public XmlModelList<MetadataSet> MetadataSets { get; protected set; }

        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Collection PrimaryCollection { get; set; }
        [Column("PrimaryCollectionId")]
        public int? PrimaryCollectionId { get; set; }

        public Entity()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();

            Initialize();

            XmlModel xml = new XmlModel(Data);

            MetadataSets = new XmlModelList<MetadataSet>(xml.GetElement("metadata-sets", true));
        }

        public virtual void Initialize()
        {
            if (Data == null)
                Data = new XElement("entity");
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            Data.SetAttributeValue("model-type", GetType().AssemblyQualifiedName);
            Name = new MultilingualElement(XmlHelper.GetElement(Data, "name", true));
            Description = new MultilingualElement(XmlHelper.GetElement(Data, "description", true));
        }

        public T InstantiateViewModel<T>() where T : XmlModel
        {
            var type = typeof(T); 
            T vm = Activator.CreateInstance(type, Data) as T;
            return vm;
        }

    }
}
