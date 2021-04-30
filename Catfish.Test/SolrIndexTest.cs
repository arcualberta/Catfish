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

        [Test]
        public void SolrAdvancedSearchTest()
        {
            List<SearchFieldConstraint> constraints = new List<SearchFieldConstraint>();

            constraints.Add(new SearchFieldConstraint()
            {
                Scope = SearchFieldConstraint.eScope.Metadata,
                ContainerId = Guid.Parse("bde40d2f-f477-4fc3-82a0-a66a36c1d39f"),
                FieldId = Guid.Parse("28ffe50b-4ec9-41b3-996f-39a5d8771dfc"),
                SearchText = "of alberta"
            });

            constraints.Add(new SearchFieldConstraint()
            {
                Scope = SearchFieldConstraint.eScope.Metadata,
                ContainerId = Guid.Parse("bde40d2f-f477-4fc3-82a0-a66a36c1d39f"),
                FieldId = Guid.Parse("8c767bc4-0456-43fa-898d-7a2f74f25061"),
                SearchText = "Cast"
            });

            constraints.Add(new SearchFieldConstraint()
            {
                Scope = SearchFieldConstraint.eScope.Metadata,
                ContainerId = Guid.Parse("bde40d2f-f477-4fc3-82a0-a66a36c1d39f"),
                FieldId = Guid.Parse("de0b29eb-b08b-43e8-9f15-ab80b09f3f81"),
                SearchText = "Sound"
            });

            var result = _solr.Search(constraints.ToArray(), 0, 50);
        }

        [Test]
        public void SolrSimpleSearchTest()
        {
            //var result = _solr.Search("of alberta", 0, 50);
            var result = _solr.Search("Alberta Folklore and Local History Collection", 0, 50);
        }
    }
}
