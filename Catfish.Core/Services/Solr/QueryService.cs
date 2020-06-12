using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class QueryService : IQueryService
    {
        private readonly ISolrReadOnlyOperations<SolrItemModel> _solr;
        public QueryService(ISolrReadOnlyOperations<SolrItemModel> qrv)
        {
            _solr = qrv;
        }

        public List<SolrItemModel> SimpleQueryByField(string fieldname, string matchword)
        {
            List<SolrItemModel> data = new List<SolrItemModel>();
            var result = _solr.Query(new SolrQuery(fieldname + ":" + matchword)); // search for "matchword" in the "fieldname" field

            foreach (var item in result)
            {
                data.Add(item);
            }

            return data;
        }

        //public ISolrQuery BuildQuery(SearchParameters parameters)
        //{
        //    if (!string.IsNullOrEmpty(parameters.FreeSearch))
        //        return new SolrQuery(parameters.FreeSearch);
        //    return SolrQuery.All;
        //}
        public ISolrQuery BuildQuery(SearchParameters parameters)
        {
            var queryList = new List<ISolrQuery>();

            //Search for a given keyword in all configured Solr fields.
            queryList.Add(new SolrQueryByField("content", parameters.FreeSearch));

            //Search for a given string in a specific field
            //queryList.Add(new SolrQueryByField("Name", parameters.Name));

            return new SolrMultipleCriteriaQuery(queryList, "OR");
        }
        public SolrQueryResults<SolrItemModel> Search(SearchParameters parameters)
        {

            var solrQueryResults = _solr.Query(BuildQuery(parameters), new QueryOptions
            {
                FilterQueries = new Collection<ISolrQuery> { Query.Field("Content").Is(parameters.FreeSearch) },
                Rows = parameters.PageSize,
                Start = parameters.PageIndex,
                OrderBy = new Collection<SortOrder> { SortOrder.Parse("EntityGuid asc") },
                Facet = new FacetParameters
                {
                    Queries = new Collection<ISolrFacetQuery> { new SolrFacetFieldQuery("EntityGuid") { MinCount = 1 } }
                }
            });
            return solrQueryResults;
        }

    }
}
