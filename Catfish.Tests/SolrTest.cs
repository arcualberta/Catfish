using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catfish.Core.Services;

namespace Catfish.Tests
{
    [TestClass]
    public class SolrTest
    {
        [TestMethod]
        public void TestFailedServiceInitialization()
        {
            try {
                SolrService.Init(null);
                Assert.Fail("Initialization passed with no connection string.");
            } catch(InvalidOperationException e)
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
