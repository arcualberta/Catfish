using Catfish.Areas.Manager.Controllers;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piranha.Areas.Manager.Views.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Catfish.Tests
{
    [TestClass]
    public class EntityTypeTest
    {
        CatfishDbContext db = new CatfishDbContext();
        [TestMethod]
        public void CreateEntityType()
        {
           
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            EntityTypeViewModel etVM = new EntityTypeViewModel();
            etVM.Name = "entity type Test3";
            etVM.Description = "Decscription for this test entity";
            //get DC metadata set
            MetadataSet metadataSet = db.MetadataSets.Where(m => m.Id == 7426).FirstOrDefault();
           
            etVM.TargetType[1] = true; //collections
            etVM.TargetType[2] = true; //items

            MetadataSetListItem mdListItem = new MetadataSetListItem();
            mdListItem.Id = metadataSet.Id;
            mdListItem.Name = metadataSet.Name;
            etVM.AssociatedMetadataSets.Add(mdListItem);
            MetadataFieldMapping mfm = new MetadataFieldMapping();
            mfm.MetadataSet = metadataSet.Name;
            mfm.MetadataSetId = metadataSet.Id;
            mfm.Field = "Title";
            etVM.NameMapping = mfm;
            mfm.Field = "Description";
            etVM.DescriptionMapping = mfm;

            EntityType model = new EntityType();
            etVM.UpdateDataModel(model, db);
            db.EntityTypes.Add(model);
            db.SaveChanges();

            EntityType newEntityType = db.EntityTypes.Where(e => e.Id == model.Id).FirstOrDefault();

            Assert.IsNotNull(newEntityType);
            Assert.AreEqual(model, newEntityType);

            Collection newCol = CreateCollection(newEntityType);
            Assert.AreNotEqual(null, newCol.Content);

            Item newItem = CreateItem(newEntityType);
            Assert.AreNotEqual(null, newItem.Content);
        }
        private Collection CreateCollection(EntityType et)
        {
            CollectionService srv = new CollectionService(db);

            Collection c = srv.CreateEntity<Collection>(et.Id);
            string name = TestData.LoremIpsum(5, 10);
            c.SetName("Collection: " + name);
            c.SetDescription("Collection test description");
            c.Serialize();

            db.Collections.Add(c);

            db.SaveChanges();
            return c;
        }

        
        public Item CreateItem(EntityType et)
        { 
            ItemService srv = new ItemService(db);

           // EntityType cType = db.EntityTypes.Where(e => e.Id == 8).FirstOrDefault();
            Item c = srv.CreateEntity<Item>(et.Id);
            c.SetName("Item created from create item test " );
            c.SetDescription("Description of test item");
            c.Serialize();
            db.Items.Add(c);
           
            db.SaveChanges();
            // Assert.AreNotEqual(null, c.Content);
            return c;
        }


    }
}
