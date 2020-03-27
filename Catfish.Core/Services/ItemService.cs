using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<CollectionContent> GetItems()
        {
            List<CollectionContent> ret = new List<CollectionContent>();

            // List of collections which acts as primary collections for the items
            var primaryCollectionIds = Db.Items.Select(it => it.PrimaryCollectionId).Distinct().ToList();


            return ret;
        }
    }
}
