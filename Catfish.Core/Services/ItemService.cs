using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ElmahCore;

namespace Catfish.Core.Services
{
    public class ItemService : DbEntityService
    {

        public ItemService(AppDbContext db, ErrorLog errorLog)
            : base(db, errorLog)
        {

        }

        /// <summary>
        /// Returns list of items grouped by their primary collection containers
        /// </summary>
        /// <returns></returns>
        public ItemListVM GetItems(int offset = 0, int max = 0)
        {
            try
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
                CollectionContent root = null;
                if (items.Where(it => it.PrimaryCollectionId.HasValue == false).Any())
                {
                    root = new CollectionContent()
                    {
                        Name = new MultilingualName(),
                        Description = new MultilingualDescription()
                    };
                    root.Name.SetContent("Root");
                    root.Description.SetContent("Items with no primary collection");
                    root.Items.AddRange(items.Where(it => it.PrimaryCollectionId == null));
                }


                // List of collections which acts as primary collections for the items
                var primaryCollectionIds = items
                    .Where(it => it.PrimaryCollectionId.HasValue)
                    .Select(it => it.PrimaryCollectionId)
                    .Distinct()
                    .ToList();
                foreach (var id in primaryCollectionIds)
                {
                    try
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
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }

                //TODO: order the collection array by the name of the collections in the default language

                if (root != null)
                {
                    ret.Collections.Insert(0, root);
                }


                //Adding the Item type templates
                var itemTemplates = Db.ItemTemplates
                    .Where(t => t.TargetType == new Item().GetType().FullName)
                    .Select(t => new EntityTemplateListEntry(t))
                    .ToList();
                ret.ItemTypes.AddRange(itemTemplates);

                //Adding the Collection type templates
                var collectionTemplates = Db.CollectionTemplates
                    .Where(t => t.TargetType == new Collection().GetType().FullName)
                    .Select(t => new EntityTemplateListEntry(t))
                    .ToList();
                ret.CollectionTypes.AddRange(collectionTemplates);

                return ret;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        
        public void UpdateItemlDataModel(Item model)
        {
            try
            {
                Item itemData = Db.Items.Where(i => i.Id == model.Id).FirstOrDefault();
                itemData.Content = model.Content;
                itemData.Created = model.Created;
                itemData.Updated = DateTime.Now;
                itemData.PrimaryCollectionId = model.PrimaryCollectionId;
                //solrIndexService.AddUpdate(new SolrItemModel(model));
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }
    }
}
