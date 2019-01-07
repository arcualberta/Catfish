using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestFixture]
    public class SolrServiceTest : BaseServiceTest
    {
        private DatabaseHelper mDh { get; set; }

        protected override void OnSetup()
        {
            mDh = new DatabaseHelper(true);
            //(new ServerHelper()).Start();
        }

        [Test]
        public void TestEscapeQueryString()
        {
            string inputString = "\"query:Test*\"*";
            string testString = "\\\"query\\:Test*\\\"*";
            string result = SolrService.EscapeQueryString(inputString);

            Assert.AreEqual(testString, result);
        }

        [Test]
        public void TestFailedServiceInitialization()
        {
            try
            {
                SolrService.Init((string)null);
                Assert.Fail("Initialization passed with no connection string.");
            }
            catch (InvalidOperationException e)
            {
                // Failed to initialize. This passes the test.
                Assert.IsTrue(true);
            }

        }
        
        [Test]
        public void CanInitializeSolr()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            SolrService.Init(connectionString);

            Assert.IsTrue(SolrService.IsInitialized);
            Assert.IsNotNull(SolrService.SolrOperations);
        }
    }
}
