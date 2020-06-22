using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents;
using Newtonsoft.Json;
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
        public static readonly string Tag = "entity";
        public static readonly string NameTag = "name";
        public static readonly string DescriptionTag = "description";


        [Key]
        public Guid Id
        {
            get =>Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }

        [JsonIgnore]
        [Column(TypeName = "xml")]
        public string Content
        {
            get => Data?.ToString();
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    Data = XElement.Parse(value);
                    Initialize(false);
                }
            }
        }

        [JsonIgnore]
        [NotMapped]
        public virtual XElement Data { get; set; }

        public DateTime Created
        {
            get => DateTime.Parse(Data.Attribute("created").Value);
            set => Data.SetAttributeValue("created", value);
        }

        public DateTime? Updated
        {
            get { try { return Data.Attribute("updated") != null ? DateTime.Parse(Data.Attribute("updated").Value) : null as DateTime?; } catch (Exception) { return null as DateTime?; } }
            set => Data.SetAttributeValue("updated", value);
        }

        public string ModelType
        {
            get => Data.Attribute("model-type").Value;
        }

        [NotMapped]
        public MultilingualText Name { get; protected set; }
        [NotMapped]
        public MultilingualText Description { get; protected set; }

        [NotMapped]
        public XmlModelList<MetadataSet> MetadataSets { get; protected set; }

        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Collection PrimaryCollection { get; set; }
        [Column("PrimaryCollectionId")]
        public Guid? PrimaryCollectionId { get; set; }

        
        public Entity()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();

            Initialize(false);
        }

        public virtual void Initialize(bool regenerateId)
        {
            if (Data == null)
                Data = new XElement(Tag);

            if (regenerateId || Data.Attribute("id") == null)
                Id = Guid.NewGuid();

            if (Data.Attribute("created") == null)
                Created = DateTime.Now;

            if (Data.Attribute("model-type") == null)
                Data.SetAttributeValue("model-type", GetType().AssemblyQualifiedName);

            //Unlike in the cases of xml-attribute-based properties, the Name and Descrition
            //properties must be initialized every time the model is initialized. 
            Name = new MultilingualText(XmlHelper.GetElement(Data, Entity.NameTag, true));
            Description = new MultilingualText(XmlHelper.GetElement(Data, Entity.DescriptionTag, true));

            //Building the Metadata Set list
            XmlModel xml = new XmlModel(Data);
            MetadataSets = new XmlModelList<MetadataSet>(xml.GetElement("metadata-sets", true));
        }

        public T InstantiateViewModel<T>() where T : XmlModel
        {
            var type = typeof(T); 
            T vm = Activator.CreateInstance(type, Data) as T;
            return vm;
        }

    }
}
