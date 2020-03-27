using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels.ListEntries
{
    public class EntityTemplateListEntry : EntityListEntry
    {
        public enum eEntityType { 
            Item, Collection, Other 
        }

        public string TypeName { get; set; }
        public string EntityType { get; set; }
        public string AddURL { get; set; }
        public EntityTemplateListEntry(EntityTemplate et)
            : base(et)
        {
            TypeName = et.TemplateName;

            var t = Type.GetType(ModelType);
            if (typeof(Collection).IsAssignableFrom(t))
            {
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Collection);
                AddURL = "manager/api/collections/add";
            }
            else if (typeof(Item).IsAssignableFrom(t))
            {
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Item);
                AddURL = "manager/api/items/add";
            }
            else
            {
                EntityType = Enum.GetName(typeof(eEntityType), eEntityType.Other);
            }
        }
    }
}
