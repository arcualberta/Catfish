using Catfish.Core.Models;
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
    public class CollectionServiceTest : BaseServiceTest
    {
        private CFCollection CreateCollection(CollectionService cs, int entityTypeId, string name, string description, bool store = false)
        {
            CFCollection c = cs.CreateCollection(entityTypeId);
            c.Name = name;
            c.Description = description;


            if (store)
            {
                c = cs.UpdateStoredCollection(c);
            }

            return c;
        }

        [Test]
        public void TestCreateCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 1";
            string description = "Description";

            CFCollection c = CreateCollection(Cs, entityTypeId, name, description);

            Assert.AreEqual(name, c.Name);
            Assert.AreEqual(description, c.Description);
        }

        [Test]
        public void TestUpdateCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 2";
            string description = "Description";
            string name2 = "Test 3";
            string description2 = "New Description";

            CFCollection c = CreateCollection(Cs, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();

            c = Cs.GetCollection(c.Id);

            int id = c.Id;
            string content = c.Content;
            Assert.AreEqual(name, c.Name);
            Assert.AreEqual(description, c.Description);
            Assert.AreNotEqual(name2, c.Name);
            Assert.AreNotEqual(description2, c.Description);

            c = Cs.CreateCollection(entityTypeId); // Since we are using an in memory database, we need to duplicate our collection.
            c.Id = id;
            c.Name = name2;
            c.Description = description2;
            Cs.UpdateStoredCollection(c);
            Dh.Db.SaveChanges();

            CFCollection c2 = Cs.GetCollection(id);

            Assert.AreNotEqual(name, c2.Name);
            Assert.AreNotEqual(description, c2.Description);
            Assert.AreEqual(name2, c2.Name);
            Assert.AreEqual(description2, c2.Description);
            Assert.AreEqual(id, c2.Id);
        }

        [Ignore("Not yet implemented")]
        // This test uses Collection Service Get Collection with Guid, this is 
        // the only place this methods is used
        [Test]
        public void TestGetCollection()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //CollectionService Cs = new CollectionService(Dh.Db);
            //int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            //string name = "Test 4";
            //string description = "Descriptiony";

            //CFCollection c = CreateCollection(Cs, entityTypeId, name, description, true);
            //Dh.Db.SaveChanges();

            //CFCollection c2 = Cs.GetCollection(c.Id);

            //int id = c.Id;
            //Assert.AreEqual(name, c2.Name);
            //Assert.AreEqual(description, c2.Description);

            //CFCollection c3 = Cs.GetCollection(c.Guid);
            //Assert.AreEqual(id, c.Id);
            //Assert.AreEqual(c2.Content, c3.Content);
        }

        [Ignore("Not yet implemented")]
        // Need to figure out how to get collections using user identity
        [Test]
        public void GetCollections()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //CollectionService Cs = new CollectionService(Dh.Db);

            //IQueryable<CFCollection> collections = Cs.GetCollections();

            //Assert.AreEqual(DatabaseHelper.TOTAL_COLLECTIONS, collections.Count());
        }

        [Ignore("Not yet implemented")]
        // Need to figure out how to get collections using user identity
        [Test]
        public void DeleteCollection()
        {
            //DatabaseHelper Dh = new DatabaseHelper(true);
            //CollectionService Cs = new CollectionService(Dh.Db);
            //Piranha.Entities.User admin = Dh.PDb.Users.First();
            //IIdentity identity = new GenericIdentity(admin.Login, Dh.PDb.Groups.Find(admin.GroupId).Name);

            //CFCollection test = Cs.GetCollections().FirstOrDefault();
            //int id = test.Id;
            //IQueryable<CFCollection> collections = Cs.GetCollections();

            //Assert.AreEqual(DatabaseHelper.TOTAL_COLLECTIONS, collections.Count());

            //Cs.DeleteCollection(id);
            //Dh.Db.SaveChanges(identity);

            //Assert.AreNotEqual(DatabaseHelper.TOTAL_COLLECTIONS, collections.Count());
            //Assert.AreEqual(DatabaseHelper.TOTAL_COLLECTIONS - 1, collections.Count());

            //try
            //{
            //    Cs.DeleteCollection(id);
            //    Dh.Db.SaveChanges(identity);
            //    Assert.Fail("An Exception should have been thrown on a bad delete.");
            //}
            //catch(ArgumentException ex)
            //{

            //}
            
            //Assert.AreNotEqual(DatabaseHelper.TOTAL_COLLECTIONS, collections.Count());
            //Assert.AreEqual(DatabaseHelper.TOTAL_COLLECTIONS - 1, collections.Count());
        }
    }
}
