using Catfish.Core.Models;
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
        public IList<SolrEntry> FreeSearch(SearchParameters parameters, int start = 0, int limit = 100);
        public IList<SolrEntry> KeywordSearch(string[] keywords, string[] categories, int start = 0, int limit = 100);

    }
}
