using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public class CollectionContent
    {
        public Guid? Id { get; set; }
        public MultilingualText Name { get; set; }
        public MultilingualDescription Description { get; set; }

        public List<EntityListEntry> Items { get; set; } = new List<EntityListEntry>();
        public List<EntityListEntry> Collections { get; set; } = new List<EntityListEntry>();
        public CollectionContent()
        {
            Name = new MultilingualName();
            Description = new MultilingualDescription();
        }

        public CollectionContent(Collection src)
        {
            Id = src.Id;
            Name = src.Name;
            Description = src.Description;
        }

    }
}