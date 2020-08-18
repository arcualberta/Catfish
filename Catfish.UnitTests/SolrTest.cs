using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services.Solr;
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
                Content = "<entity id=\"a884a47b-0c18-418b-bec9-1f7392646fe2\" created=\"2020-05-15T08:51:18.6268485-06:00\" model-type=\"Catfish.Core.Models.Item, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\">",
                Created = DateTime.Now,
                Updated = DateTime.Now,
                PrimaryCollectionId = null,
            };
            solrIndexService.AddUpdate(new SolrItemModel(model));
        }

    }
}
