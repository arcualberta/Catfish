using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class CollectionServiceTest
    {
        private Collection CreateCollection(CollectionService cs, int entityTypeId, string name, string description, bool store = false)
        {
            Collection c = cs.CreateCollection(entityTypeId);
            c.Name = name;
            c.Description = description;


            if (store)
            {
                c = cs.UpdateStoredCollection(c);
            }

            return c;
        }

        [TestMethod]
        public void TestCreateCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 1";
            string description = "Description";

            Collection c = CreateCollection(Cs, entityTypeId, name, description);

            Assert.AreEqual(name, c.Name);
            Assert.AreEqual(description, c.Description);
        }

        [TestMethod]
        public void TestUpdateCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 2";
            string description = "Description";
            string name2 = "Test 3";
            string description2 = "New Description";

            Collection c = CreateCollection(Cs, entityTypeId, name, description, true);
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

            Collection c2 = Cs.GetCollection(id);

            Assert.AreNotEqual(name, c2.Name);
            Assert.AreNotEqual(description, c2.Description);
            Assert.AreEqual(name2, c2.Name);
            Assert.AreEqual(description2, c2.Description);
            Assert.AreEqual(id, c2.Id);
        }

        [TestMethod]
        public void TestGetCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 4";
            string description = "Descriptiony";

            Collection c = CreateCollection(Cs, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();

            Collection c2 = Cs.GetCollection(c.Id);

            int id = c.Id;
            Assert.AreEqual(name, c2.Name);
            Assert.AreEqual(description, c2.Description);
        }
    }
}
