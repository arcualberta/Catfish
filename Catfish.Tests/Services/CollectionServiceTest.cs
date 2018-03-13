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


            if (store)
            {
                cs.UpdateStoredCollection(c);
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
        }

        [TestMethod]
        public void TestUdpateCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);
            int entityTypeId = Dh.Db.EntityTypes.Where(e => e.TargetTypes.Contains("Collection")).Select(e => e.Id).FirstOrDefault();
            string name = "Test 2";
            string description = "Description";

            Collection c = CreateCollection(Cs, entityTypeId, name, description, true);
            Dh.Db.SaveChanges();
        }

        [TestMethod]
        public void TestGetCollection()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            CollectionService Cs = new CollectionService(Dh.Db);

            // Create the needed Collection


            // Test the results
        }
    }
}
