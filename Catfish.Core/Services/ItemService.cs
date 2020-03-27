using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Catfish.Core.Services
{
    public class ItemService : DbEntityService
    {
        public ItemService(AppDbContext db)
            : base(db)
        {
            
        }

        /// <summary>
        /// Returns list of items grouped by their primary collection containers
        /// </summary>
        /// <returns></returns>
        public ItemListVM GetItems(int offset = 0, int max = 0)
        {
            ItemListVM ret = new ItemListVM() { OffSet = offset, Max = max };

            if (max == 0)
                max = Int32.MaxValue;

            var items = Db.Items
                .Skip(offset)
                .Take(max)
                .Include(it => it.PrimaryCollection)
                .Select(it => new EntityListEntry(it))
                .ToList();

            //List of items that does not have a primary collection
            CollectionContent nullCollection = null;
            if (items.Where(it => it.PrimaryCollectionId.HasValue == false).Any())
            {
                nullCollection = new CollectionContent()
                {
                    Name = new MultilingualText(Entity.NameTag),
                    Description = new MultilingualText(Entity.DescriptionTag)
                };
                nullCollection.Name.SetContent("Root");
                nullCollection.Description.SetContent("Items with no primary collection");
                nullCollection.Items.AddRange(items.Where(it => it.PrimaryCollectionId == null));
            }


            // List of collections which acts as primary collections for the items
            var primaryCollectionIds = items
                .Where(it => it.PrimaryCollectionId.HasValue)
                .Select(it => it.PrimaryCollectionId)
                .Distinct()
                .ToList();
            foreach(var id in primaryCollectionIds)
            {
                var itemSubSet = items.Where(it => it.PrimaryCollectionId == id);
                CollectionContent c = new CollectionContent()
                {
                    Id = itemSubSet.First().PrimaryCollectionId,
                    Name = itemSubSet.First().PrimaryCollectionName
                };
                c.Items.AddRange(itemSubSet);
                ret.Collections.Add(c);
            }

            //TODO: order the collection array by the name of the collections in the default language

            if(nullCollection != null)
            {
                ret.Collections.Insert(0, nullCollection);
            }


            //Adding the Item type templates
            var itemTemplates = Db.ItemTemplates
                .Where(t => t.TargetType == new Item().GetType().FullName)
                .Select(t => new EntityTemplateListEntry(t))
                .ToList();
            ret.ItemTypes.AddRange(itemTemplates);

            return ret;
        }
    }
}
