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
    public class SolrServiceTest
    {
        private DatabaseHelper mDh { get; set; }

        [ClassInitialize]
        public void InitializeTesting()
        {
            //mDh = new DatabaseHelper(true);
        }

        [TestMethod]
        public void TestEscapeQueryString()
        {
            string inputString = "\"query:Test*\"*";
            string testString = "\\\"query\\:Test*\\\"*";
            string result = SolrService.EscapeQueryString(inputString);

            Assert.AreEqual(testString, result);
        }

        [TestMethod]
        public void TestFailedServiceInitialization()
        {
            try
            {
                SolrService.Init(null);
                Assert.Fail("Initialization passed with no connection string.");
            }
            catch (InvalidOperationException e)
            {
                // Failed to initialize. This passes the test.
            }

        }

        [TestMethod]
        public void TestSuccessServiceInitialization()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            SolrService.Init(connectionString);
        }
    }
}
