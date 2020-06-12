using Catfish.Core.Models.Solr;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public interface IQueryService
    {
        List<SolrItemModel> SimpleQueryByField(string fieldname, string matchword);
        ISolrQuery BuildQuery(SearchParameters parameters);
        SolrQueryResults<SolrItemModel> Search(SearchParameters parameters);
    }
}
