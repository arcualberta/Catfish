using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catfish.Core.Models;
using System.Collections.Generic;
using System;
using Catfish.Core.Services;

namespace Catfish.Tests
{
    [TestClass]
    public class RandomDataIngest
    {
        [TestMethod]
        public void CreateCollections()
        {
            int COUNT = 20;

            CatfishDbContext db = new CatfishDbContext();
            List<EntityType> collectionTypes = db.EntityTypes.Where(t => t.TargetType == EntityType.eTarget.Collections).ToList();
            if (collectionTypes.Count == 0)
                throw new Exception("No entity types have been defined for collections.");
            var rand = new Random();
            CollectionService srv = new CollectionService(db);
            for(int i=0; i<COUNT; ++i)
            {
                EntityType cType = collectionTypes[rand.Next(0, collectionTypes.Count)];
                Collection c = srv.CreateEntity<Collection>(cType.Id);
                string name = TestData.LoremIpsum(5, 10);
                c.SetName("Collection: " + name);
                db.Collections.Add(c);
            }
            db.SaveChanges();
        }

        [TestMethod]
        public void CreateItems()
        {
            int COUNT = 50;

            CatfishDbContext db = new CatfishDbContext();
            List<EntityType> itemTypes = db.EntityTypes.Where(t => t.TargetType == EntityType.eTarget.Items).ToList();
            if (itemTypes.Count == 0)
                throw new Exception("No entity types have been defined for collections.");
            var rand = new Random();
            ItemService srv = new ItemService(db);
            for (int i = 0; i < COUNT; ++i)
            {
                EntityType cType = itemTypes[rand.Next(0, itemTypes.Count)];
                Item c = srv.CreateEntity<Item>(cType.Id);
                string name = TestData.LoremIpsum(5, 10);
                c.SetName("Item: " + name);
                db.Items.Add(c);
            }
            db.SaveChanges();
        }

    }
}
