using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class ItemServiceTest
    {
        private CFItem CreateItem(ItemService itemSrv, int entityTypeId, string name, string description, bool store = false)
        {
            CFItem i = itemSrv.CreateItem(entityTypeId);
            i.Name = name;
            i.Description = description;


            if (store)
            {
                i = itemSrv.UpdateStoredItem(i);
            }

            return i;
        }

        [TestMethod]
        public void TestCreateItem()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Item")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 1";
            string description = "Description";

            CFItem i = CreateItem(Is, entityTypeId, name, description);

            Assert.AreEqual(name, i.Name);
            Assert.AreEqual(description, i.Description);
        }

        [TestMethod]
        public void TestUpdateItem()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Item")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 2";
            string description = "Description";
            string name2 = "Test 3";
            string description2 = "New Description";

            CFItem i = CreateItem(Is, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();

            i = Is.GetItem(i.Id);

            int id = i.Id;
            string content = i.Content;
            Assert.AreEqual(name, i.Name);
            Assert.AreEqual(description, i.Description);
            Assert.AreNotEqual(name2, i.Name);
            Assert.AreNotEqual(description2, i.Description);

            i = Is.CreateItem(entityTypeId); // Since we are using an in memory database, we need to duplicate our collection.
            i.Id = id;
            i.Name = name2;
            i.Description = description2;
            Is.UpdateStoredItem(i);
            Dh.Db.SaveChanges();

            CFItem i2 = Is.GetItem(id);

            Assert.AreNotEqual(name, i2.Name);
            Assert.AreNotEqual(description, i2.Description);
            Assert.AreEqual(name2, i2.Name);
            Assert.AreEqual(description2, i2.Description);
            Assert.AreEqual(id, i2.Id);
        }

        [TestMethod]
        public void TestGetItem()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Item")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 4";
            string description = "Descriptiony";

            CFItem i = CreateItem(Is, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();

            CFItem i2 = Is.GetItem(i.Id);

            int id = i.Id;
            Assert.AreEqual(name, i2.Name);
            Assert.AreEqual(description, i2.Description);

            CFItem i3 = Is.GetItem(i.Guid);
            Assert.AreEqual(id, i.Id);
            Assert.AreEqual(i2.Content, i3.Content);
        }

        [TestMethod]
        public void GetItems()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);

            IQueryable<CFItem> items = Is.GetItems();

            Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
        }

        [TestMethod]
        public void DeleteItem()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);
            Piranha.Entities.User admin = Dh.PDb.Users.First();
            IIdentity identity = new GenericIdentity(admin.Login, Dh.PDb.Groups.Find(admin.GroupId).Name);

            CFItem test = Is.GetItems().FirstOrDefault();
            int id = test.Id;
            IQueryable<CFItem> items = Is.GetItems();

            Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());

            Is.DeleteItem(id);
            Dh.Db.SaveChanges(identity);

            Assert.AreNotEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
            Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS - 1, items.Count());

            try
            {
                Is.DeleteItem(id);
                Dh.Db.SaveChanges(identity);
                Assert.Fail("An Exception should have been thrown on a bad delete.");
            }
            catch (ArgumentException ex)
            {

            }

            Assert.AreNotEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
            Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS - 1, items.Count());
        }
    }
}
