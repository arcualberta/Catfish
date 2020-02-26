using Catfish.Core.Models;
using Catfish.Tests.Helpers;
using NUnit.Framework;

namespace Catfish.Tests
{
    public class ProjectMigrationTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestServiceInstantiations()
        {
            TestHelper helper = new TestHelper();
            ////EntityService ent = helper.Seviceprovider.GetService(typeof(EntityService)) as EntityService;
            ////SolrService slr = helper.Seviceprovider.GetService(typeof(SolrService)) as SolrService;

            Assert.Pass();
        }
    }
}