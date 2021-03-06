﻿using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestFixture]
    public class ItemServiceTest : BaseServiceTest
    {
        private CFItem CreateItem(ItemService itemSrv, int entityTypeId, string name, string description, bool store = false)
        {
            CFItem i = itemSrv.CreateItem(entityTypeId);
            i.SetName(name);
            i.SetDescription(description);


            if (store)
            {
                i = itemSrv.UpdateStoredItem(i);
            }

            return i;
        }

        [Test]
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

        [Ignore("Not yet implemented")]
        // This test is using the Item service GetItem method which requires 
        // a IIdentity as parameter
        [Test]
        public void TestUpdateItem()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //ItemService Is = new ItemService(Dh.Db);
            //int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Item")).Select(e => e.Id).FirstOrDefault();
            //string name = "Test 2";
            //string description = "Description";
            //string name2 = "Test 3";
            //string description2 = "New Description";

            //CFItem i = CreateItem(Is, entityTypeId, name, description, true);
            //Dh.Db.SaveChanges();

            //i = Is.GetItem(i.Id);

            //int id = i.Id;
            //string content = i.Content;
            //Assert.AreEqual(name, i.Name);
            //Assert.AreEqual(description, i.Description);
            //Assert.AreNotEqual(name2, i.Name);
            //Assert.AreNotEqual(description2, i.Description);

            //i = Is.CreateItem(entityTypeId); // Since we are using an in memory database, we need to duplicate our collection.
            //i.Id = id;
            //i.Name = name2;
            //i.Description = description2;
            //Is.UpdateStoredItem(i);
            //Dh.Db.SaveChanges();

            //CFItem i2 = Is.GetItem(id);

            //Assert.AreNotEqual(name, i2.Name);
            //Assert.AreNotEqual(description, i2.Description);
            //Assert.AreEqual(name2, i2.Name);
            //Assert.AreEqual(description2, i2.Description);
            //Assert.AreEqual(id, i2.Id);
        }
        
        [Test]
        public void TestGetItem()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            ItemService Is = new ItemService(Dh.Db);

            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Item")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 4";
            string description = "Description";

            CFItem item = CreateItem(Is, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();

            CFItem fetchedItem = Is.GetItem(item.Id);

            int id = item.Id;
            Assert.AreEqual(name, fetchedItem.Name);
            Assert.AreEqual(description, fetchedItem.Description);
            Assert.AreEqual(item.Content, fetchedItem.Content);
            
        }

        [Ignore("Not yet implemented")]
        // Need to use IIdentity for GetItems
        [Test]
        public void GetItems()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //ItemService Is = new ItemService(Dh.Db);

            //IQueryable<CFItem> items = Is.GetItems();

            //Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
        }

        [Ignore("Not yet implemented")]
        // Need to use IIdentity for GetItems
        [Test]
        public void DeleteItem()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //ItemService Is = new ItemService(Dh.Db);
            //Piranha.Entities.User admin = Dh.PDb.Users.First();
            //IIdentity identity = new GenericIdentity(admin.Login, Dh.PDb.Groups.Find(admin.GroupId).Name);

            //CFItem test = Is.GetItems().FirstOrDefault();
            //int id = test.Id;
            //IQueryable<CFItem> items = Is.GetItems();

            //Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());

            //Is.DeleteItem(id);
            //Dh.Db.SaveChanges(identity);

            //Assert.AreNotEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
            //Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS - 1, items.Count());

            //try
            //{
            //    Is.DeleteItem(id);
            //    Dh.Db.SaveChanges(identity);
            //    Assert.Fail("An Exception should have been thrown on a bad delete.");
            //}
            //catch (ArgumentException ex)
            //{

            //}

            //Assert.AreNotEqual(DatabaseHelper.TOTAL_ITEMS, items.Count());
            //Assert.AreEqual(DatabaseHelper.TOTAL_ITEMS - 1, items.Count());
        }
    }
}
