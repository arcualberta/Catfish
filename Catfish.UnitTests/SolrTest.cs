using Catfish.Core.Models;
using Catfish.Solr;
using Catfish.Solr.Models;
using NUnit.Framework;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.UnitTests
{   
    public class SolrTest
    {
        private ISolrIndexService<SolrItemModel> solrIndexService;

        [Test]
        public void SolrAdd()
        {
            Guid guidobj = Guid.NewGuid();  // Generates a unique identifier
            var model = new Item
            {
                
                Id = guidobj,
                Content = null,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                PrimaryCollectionId = null,
            };
            solrIndexService.AddUpdate(new SolrItemModel(model));
        }

    }
}
