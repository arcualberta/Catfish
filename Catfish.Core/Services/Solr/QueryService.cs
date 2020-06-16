using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Mapping.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class QueryService : IQueryService
    {
        private readonly ISolrReadOnlyOperations<SolrItemModel> _solr;
        private readonly AppDbContext _db;
        public QueryService(ISolrReadOnlyOperations<SolrItemModel> qrv, AppDbContext db)
        {
            _solr = qrv;
            _db = db;
        }

        public SolrQueryResults<SolrItemModel> SimpleQueryByField1(string fieldname, string matchword)
        {
            SolrQueryResults<SolrItemModel> data = new SolrQueryResults<SolrItemModel>();
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

            
            return new SolrMultipleCriteriaQuery(queryList, "OR");
        }

        public SolrQueryResults<SolrItemModel> Search(SearchParameters parameters)
        {

            var solrQueryResults = _solr.Query(SolrQuery.All, new QueryOptions
            {
                FilterQueries = new Collection<ISolrQuery> { Query.Field("content").Is(parameters.FreeSearch) },
                Rows = parameters.PageSize,
                Start = parameters.PageIndex,
                //OrderBy = new Collection<SortOrder> { SortOrder.Parse("entityGuid asc") },
                //Facet = new FacetParameters
                //{
                //    Queries = new Collection<ISolrFacetQuery> { new SolrFacetFieldQuery("entityGuid") { MinCount = 1 } }
                //}
            });
            return solrQueryResults;
        }
        public SolrQueryResults<SolrItemModel> Results(SearchParameters parameters)
        {
            //QueryOptions query_options = new QueryOptions
            //{
            //    Rows = 10,
            //    StartOrCursor = new StartOrCursor.Start(0),
            //    FilterQueries = new ISolrQuery[] {
            //    new SolrQueryByField("content","provides"),
            //    }
            //};
            //// Construct the query
            //SolrQuery query = new SolrQuery("provides");
            //// Run a basic keyword search, filtering for questions only
            //var posts = _solr.Query(query, query_options);
            //SolrQueryResults<SolrItemModel> data = new SolrQueryResults<SolrItemModel>();
            //foreach (var item in posts)
            //{
            //    data.Add(item);
            //}

            var q = new SolrQuery("content:" + parameters.FreeSearch);

            var res = _solr.Query(q);

            SolrQueryResults<SolrItemModel> products2 = _solr.Query(q);

            return products2;
        }

        public IList<Entity> GetEntities(SearchParameters parameters)
        {
            SolrQueryResults<SolrItemModel> result = Results(parameters);
            var result_ids = result.Select(s => s.EntityGuid.FirstOrDefault()).ToList();
            var entities = _db.Entities.Where(e => result_ids.Contains(e.Id)).ToList();

            return entities;
        }


    }
}
