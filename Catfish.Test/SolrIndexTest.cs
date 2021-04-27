using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Test.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Catfish.Test
{
    public class SolrIndexTest
    {
        private AppDbContext _db;
        private TestHelper _testHelper;
        private ISolrService _solr;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            _solr = _testHelper.SolrService;
        }

        [Test]
        public void SolrDocInitializationTest()
        {
            string filename = @"C:\Projects\Catfish-2.0-Production-Data-Samples\item.xml";
            Assert.IsTrue(File.Exists(filename));
            Item item = new Item();
            item.Content = File.ReadAllText(filename);

            item.Id = Guid.NewGuid();
            item.Created = DateTime.Now;

            _solr.Index(item);
        }


    }
}
