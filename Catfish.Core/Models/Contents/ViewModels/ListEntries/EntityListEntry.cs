﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels.ListEntries
{
    public class EntityListEntry
    {
        public Guid Id { get; set; }
        public string ModelType { get; set; }
        public MultilingualText Name { get; protected set; }
        public MultilingualText Description { get; protected set; }
        public string ConcatenatedName { get; protected set; }

        [JsonIgnore]
        public Guid? TemplateId { get; set; }
        [JsonIgnore]
        public string ItemLabel { get; set; }

        [JsonIgnore]
        public Guid? PrimaryCollectionId { get; set; }

        [JsonIgnore]
        public MultilingualText PrimaryCollectionName { get; set; }
        [JsonIgnore]
        public bool IsItem { get; set; }

        public EntityListEntry(Entity entity)
        {
            Id = entity.Id;
            ModelType = entity.ModelType;
            Name = entity.Name;
            ConcatenatedName = entity.ConcatenatedName;
            Description = entity.Description;
            PrimaryCollectionId = entity.PrimaryCollectionId;
            PrimaryCollectionName = entity.PrimaryCollection != null ? entity.PrimaryCollection.Name : null;
            TemplateId = entity.TemplateId != null ? entity.TemplateId : null;

            IsItem = ModelType.Contains("Catfish.Core.Models.Item") ? true : false;
            
        }

    }
}
