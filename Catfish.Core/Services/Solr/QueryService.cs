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
        private readonly ISolrReadOnlyOperations<SolrItemModel> _solrItemQuery;
        private readonly ISolrReadOnlyOperations<SolrPageContentModel> _solrPageQuery;
        private readonly AppDbContext _db;
        public QueryService(ISolrReadOnlyOperations<SolrItemModel> qrvItem, ISolrReadOnlyOperations<SolrPageContentModel> qrvPage, AppDbContext db)
        {
            _solrItemQuery = qrvItem;
            _solrPageQuery = qrvPage;
            _db = db;
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

            var solrQueryResults = _solrItemQuery.Query(SolrQuery.All, new QueryOptions
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

            //var q = new SolrQuery("content:" + parameters.FreeSearch);

            //var res = _solr.Query(new SolrQuery("content:" + parameters.FreeSearch));

            SolrQueryResults<SolrItemModel> products2 = _solrItemQuery.Query(new SolrQuery("content:" + parameters.FreeSearch));

            return products2;
        }

        public IList<Entity> GetEntities(SearchParameters parameters)
        {
            SolrQueryResults<SolrItemModel> result = Results(parameters);
            var result_ids = result.Select(s => s.EntityGuid.FirstOrDefault()).ToList();
            var entities = _db.Entities.Where(e => result_ids.Contains(e.Id)).ToList();

            return entities;
        }

        public IList<SolrEntry> GetPages(SearchParameters parameters, int start = 0, int limit = 100)
        {
            var query = new SolrQuery("title:" + parameters.FreeSearch).Boost(2) +
                        new SolrQuery("excerpt:" + parameters.FreeSearch) +
                        new SolrQuery("blockContent:" + parameters.FreeSearch);

            //Result hilighting: https://lucene.apache.org/solr/guide/8_5/highlighting.html
            var queryResult = _solrPageQuery.Query(query,
                new QueryOptions
                {
                    Fields = new[] { "id", "title", "cf_object_type" },
                    StartOrCursor = new StartOrCursor.Start(start),
                    Rows = limit,
                    ExtraParams = new Dictionary<string, string> {
                        {"hl.method", "unified" }, //Unified highligher, which is said to be new and fast
                        {"hl", "true" }, //Enable snippet highlighting
                        {"hl.fl", "*" }, //Hilight matching snippets in all fields
                        {"hl.snippets", "1" }, //Highlight up to 10 snippets. TODO: pass this as an optional config parameter
                        {"hl.tag.pre", "<em class='bg-warning'>" }, //Start tag for hilighting matching snippets
                        {"hl.tag.post", "</em>" } //End tag for hilighting matching snippets
                    }
                });

            var highlights = queryResult.Highlights.ToList();

            //List<SolrEntry> result = queryResult.Select(qr => new SolrEntry()
            //{
            //    ObjectId = qr.Id,
            //    ObjectType = qr.ContenType.FirstOrDefault()
            //}).ToList();

            List<SolrEntry> result = new List<SolrEntry>();
            for (int i = 0; i < queryResult.Count; ++i)
            {
                var qr = queryResult[i];
                var hl = highlights[i];

                SolrEntry entry = new SolrEntry()
                {
                    Id = qr.Id,
                    ObjectType = qr.ContenType,
                    PageContent = new SolrPageContentModel(hl)
                };

                result.Add(entry);
            }

            return result;
        }


    }
}
