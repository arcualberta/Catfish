﻿using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Table("Catfish_Entities")]
    public class Entity
    {
        public static readonly string Tag = "entity";
        public static readonly string MetadataSetsRootTag = "metadata-sets";
        public static readonly string DataContainerRootTag = "data-container";

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
            get => GetDateTimeAttribute("created").Value;
            set => Data.SetAttributeValue("created", value);
        }

        [NotMapped]
        public DateTime? Updated
        {
            get => GetDateTimeAttribute("updated");
            set => Data.SetAttributeValue("updated", value);
        }

        //[NotMapped]
        public Guid? TemplateId
        {
            get => GetGuidAttribute("template-id"); 
            set => Data.SetAttributeValue("template-id", value);
        }

        [NotMapped]
        public Guid? StateId
        {
            get => GetGuidAttribute("state-id");
            set => Data.SetAttributeValue("state-id", value);
        }

        [NotMapped]
        public string ModelType
        {
            get => Data.Attribute("model-type").Value;
        }

        [NotMapped]
        public MultilingualName Name { get; protected set; }
        [NotMapped]
        public MultilingualDescription Description { get; protected set; }

        [NotMapped]
        public XmlModelList<MetadataSet> MetadataSets { get; protected set; }

        [NotMapped]
        public XmlModelList<DataItem> DataContainer { get; protected set; }

        public ICollection<Relationship> SubjectRelationships { get; set; }
        public ICollection<Relationship> ObjectRelationships { get; set; }

        public Collection PrimaryCollection { get; set; }
        [Column("PrimaryCollectionId")]
        public Guid? PrimaryCollectionId { get; set; }
        
        public SystemStatus Status { get; set; }
        [Column("StatusId")]
        public Guid? StatusId { get; set; }


        public Entity()
        {
            SubjectRelationships = new List<Relationship>();
            ObjectRelationships = new List<Relationship>();

            Initialize(false);
        }

        public string GetAttribute(string key)
        {
            var att = Data.Attribute(key);
            return (att == null) ? null : att.Value;
        }

        public Guid? GetGuidAttribute(string key)
        {
            var att = Data.Attribute(key);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? null as Guid? : Guid.Parse(att.Value);
        }

        public DateTime? GetDateTimeAttribute(string key)
        {
            var att = Data.Attribute(key);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? null as DateTime? : DateTime.Parse(att.Value);
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
            Name = new MultilingualName(XmlHelper.GetElement(Data, MultilingualName.TagName, true));
            Description = new MultilingualDescription(XmlHelper.GetElement(Data, MultilingualDescription.TagName, true));

            //Wrapping the XElement "Data" in an XmlModel wrapper so that it can be used by the
            //rest of this initialization routine.
            XmlModel xml = new XmlModel(Data);

            //Building the Metadata Set list
            MetadataSets = new XmlModelList<MetadataSet>(xml.GetElement(MetadataSetsRootTag, true), true);

            //Building the DataContainer
            DataContainer = new XmlModelList<DataItem>(xml.GetElement(DataContainerRootTag, true), true);
        }

        public T InstantiateViewModel<T>() where T : XmlModel
        {
            var type = typeof(T); 
            T vm = Activator.CreateInstance(type, Data) as T;
            return vm;
        }

        public void ReplaceMetadataSetContainer(XElement newMetadataSetContainer, bool populateChildren = false)
        {
            XmlModel xml = new XmlModel(Data);
            xml.ReplaceOrInsert(newMetadataSetContainer);

            if (populateChildren)
                MetadataSets = new XmlModelList<MetadataSet>(xml.GetElement(MetadataSetsRootTag, true), true);
        }

        public void ReplaceDataSetContainer(XElement newDataSetContainer, bool populateChildren = false)
        {
            XmlModel xml = new XmlModel(Data);
            xml.ReplaceOrInsert(newDataSetContainer);

            if (populateChildren)
                DataContainer = new XmlModelList<DataItem>(xml.GetElement(DataContainerRootTag, true), true);
        }

        public DataItem GetDataItem(string dataItemName, bool createIfNotExists, string nameLang = "en")
        {
            DataItem dataItem = this.DataContainer
                .Where(di => di.GetName(nameLang) == dataItemName)
                .FirstOrDefault();

            if (dataItem == null && createIfNotExists)
            {
                dataItem = new DataItem();
                dataItem.SetName(dataItemName, nameLang);
                DataContainer.Add(dataItem);
            }
            return dataItem;
        }

        public DataItem GetDataItem(Guid dataItemId)
        {
            return DataContainer
                .Where(di => di.Id == dataItemId)
                .FirstOrDefault();
        }

        public DataItem GetRootDataItem(bool createIfNotExists)
        {
            DataItem dataItem = this.DataContainer
                .Where(di => di.IsRoot)
                .FirstOrDefault();

            if (dataItem == null && createIfNotExists)
            {
                dataItem = new DataItem() { IsRoot = true };
                DataContainer.Add(dataItem);
            }
            return dataItem;
        }
    }
}
