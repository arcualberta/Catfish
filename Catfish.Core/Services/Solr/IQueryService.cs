﻿using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public interface IQueryService
    {       
        ISolrQuery BuildQuery(SearchParameters parameters);
        SolrQueryResults<SolrItemModel> Search(SearchParameters parameters);
        SolrQueryResults<SolrItemModel> Results(SearchParameters parameters);
        //List<SolrItemModel> Search(string searchTerm);
        public IList<Entity> GetEntities(SearchParameters parameters);
        public IList<SolrPageContentModel> GetPages(SearchParameters parameters);

    }
}
