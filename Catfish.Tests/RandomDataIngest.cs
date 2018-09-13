using System.Linq;
using NUnit.Framework;
using Catfish.Core.Models;
using System.Collections.Generic;
using System;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;

namespace Catfish.Tests
{
    //[TestFixture]
    public class RandomDataIngest : BaseUnitTest
    {
        [Ignore("Not yet implemented")] // Done to prevent the test from running each time we do a full test.
        [Test]
        public void CreateCollections()
        {
            int COUNT = 20;

            CatfishDbContext db = new CatfishDbContext();
            List<CFEntityType> collectionTypes = db.EntityTypes.Where(t => t.TargetTypes.Contains(CFEntityType.eTarget.Collections.ToString())).ToList();//db.EntityTypes.Where(t => t.TargetType == EntityType.eTarget.Collections).ToList();
            if (collectionTypes.Count == 0)
                throw new Exception("No entity types have been defined for collections.");
            var rand = new Random();
            CollectionService srv = new CollectionService(db);
            for(int i=0; i<COUNT; ++i)
            {
                CFEntityType cType = collectionTypes[rand.Next(0, collectionTypes.Count)];
                CFCollection c = srv.CreateEntity<CFCollection>(cType.Id);
                string name = TestData.LoremIpsum(5, 10);
                c.SetName("Collection: " + name);
                c.SetDescription(TestData.LoremIpsum(20, 100, 1, 10));
                c.Serialize();
                db.Collections.Add(c);
            }
            db.SaveChanges();
        }

        [Test]
        public void CreateItems()
        {
            int COUNT = 50;

            CatfishDbContext db = new CatfishDbContext();
            List<CFEntityType> itemTypes = db.EntityTypes.Where(t => t.TargetTypes.Contains(CFEntityType.eTarget.Items.ToString())).ToList(); //db.EntityTypes.Where(t => t.TargetType == EntityType.eTarget.Items).ToList();
            if (itemTypes.Count == 0)
                throw new Exception("No entity types have been defined for collections.");
            var rand = new Random();
            ItemService srv = new ItemService(db);
            for (int i = 0; i < COUNT; ++i)
            {
                CFEntityType cType = itemTypes[rand.Next(0, itemTypes.Count)];
                CFItem c = srv.CreateEntity<CFItem>(cType.Id);
                string name = TestData.LoremIpsum(5, 10);
                c.SetDescription(TestData.LoremIpsum(20, 100, 1, 10));
                c.SetName("Item: " + name);
                c.Serialize();
                db.Items.Add(c);
            }
            db.SaveChanges();
        }

        protected override void OnSetup()
        {
        }

        protected override void OnTearDown()
        {
        }
    }
}
