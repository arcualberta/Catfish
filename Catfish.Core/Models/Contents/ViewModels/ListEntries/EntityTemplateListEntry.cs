using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels.ListEntries
{
    public class EntityTemplateListEntry
    {
        public enum eEntityType { 
            Item, Collection, Other 
        }

        public string ModelType { get; set; }
        public string TypeName { get; set; }
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public EntityTemplateListEntry(EntityTemplate et)
        {
            ModelType = et.ModelType;
            TypeName = et.TypeName;
            Id = et.Id;

            var t = Type.GetType(ModelType);
            if (typeof(Collection).IsAssignableFrom(t))
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Collection);
            else if (typeof(Item).IsAssignableFrom(t))
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Item);
            else
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Other);
        }
    }
}
