using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public class ItemListVM
    {
        public List<CollectionContent> Collections { get; set; } = new List<CollectionContent>();
        public List<EntityTemplateListEntry> ItemTypes { get; set; } = new List<EntityTemplateListEntry>();
        public List<EntityTemplateListEntry> CollectionTypes { get; set; } = new List<EntityTemplateListEntry>();

        public int OffSet { get; set; }
        public int Max { get; set; }
    }
}
