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
        private void CreateCollection(string name, string description, IEnumerable<MetadataSet> sets)
        {

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
