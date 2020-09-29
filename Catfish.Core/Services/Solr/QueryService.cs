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
        private readonly ISolrReadOnlyOperations<SolrEntry> _solrPageQuery;
        private readonly AppDbContext _db;
        public QueryService(ISolrReadOnlyOperations<SolrItemModel> qrvItem, ISolrReadOnlyOperations<SolrEntry> qrvPage, AppDbContext db)
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

        public IList<SolrEntry> FreeSearch(SearchParameters parameters, int start = 0, int limit = 100)
        {
            var query = new SolrQuery("title_s:" + parameters.FreeSearch) +
                        new SolrQuery("excerpt_s:" + parameters.FreeSearch) +
                        new SolrQuery("content:" + parameters.FreeSearch);

            //Result hilighting: https://lucene.apache.org/solr/guide/8_5/highlighting.html
            string highlightStartTag = "<em class='bg-warning'>";
            string highlightEndTag = "</em>";

            var queryResult = _solrPageQuery.Query(query,
                new QueryOptions
                {
                    Fields = new[] { "id", "title_s", "object_type_i", "language_s", "permalink_s", "containerId" },
                    StartOrCursor = new StartOrCursor.Start(start),
                    Rows = limit,
                    ExtraParams = new Dictionary<string, string> {
                        {"hl.method", "unified" }, //Unified highligher, which is said to be new and fast
                        {"hl", "true" }, //Enable snippet highlighting
                        {"hl.fl", "*" }, //Hilight matching snippets in all fields
                        {"hl.snippets", "5" }, //Highlight up to 10 snippets. TODO: pass this as an optional config parameter
                        {"hl.tag.pre", highlightStartTag }, //Start tag for hilighting matching snippets
                        {"hl.tag.post", highlightEndTag } //End tag for hilighting matching snippets
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
                var hkeys = highlights[i].Value.Keys.ToList();
                var hvals = highlights[i].Value.Values.ToList();
                for(int k =0; k<hkeys.Count; ++k)
                {
                    var snippets = hvals[k];
                    if (hkeys[k] == "title_s")
                    {
                        //The title filed matches the search criteria. In this
                        //case, we find the highlighted portion in the highlight
                        //and highlight the correcponding section in the actual title.

                        foreach(var snippet in snippets)
                            qr.Title = HighlightSections(snippet, highlightStartTag, highlightEndTag, qr.Title);
                    }
                    else
                    {
                        foreach (var snippet in snippets)
                            qr.Highlights.Add(snippet);
                    }
                }

                result.Add(qr);
            }

            return result;
        }

        private string HighlightSections(string snippetWithHighlights, string highlightStartTag, string highlightEndTag, string stringToBeHighlighted)
        {
            while(true)
            {
                int start = snippetWithHighlights.IndexOf(highlightStartTag);
                int end = snippetWithHighlights.IndexOf(highlightEndTag, Math.Min(snippetWithHighlights.Length, highlightStartTag.Length));
                if (start < 0 || end < 0)
                    return stringToBeHighlighted;

                start = start + highlightStartTag.Length;
                string highlightedFragmentWithoutMarkers = snippetWithHighlights.Substring(start, end - start);
                string highlightedFragmentWithMarkers = highlightStartTag + highlightedFragmentWithoutMarkers + highlightEndTag;

                //replacing all occurrances of the highlighted fragment within the stringToBeHighlighted
                stringToBeHighlighted = stringToBeHighlighted.Replace(highlightedFragmentWithoutMarkers, highlightedFragmentWithMarkers);

                //removing all occurances of the highlightedFragmentWithMarkers from the snippet
                snippetWithHighlights = snippetWithHighlights.Replace(highlightedFragmentWithMarkers, "");

                //Recursively calling the HighlightSections method to process other highlighted sections
                return HighlightSections(snippetWithHighlights, highlightStartTag, highlightEndTag, stringToBeHighlighted);
            }
        }


    }
}
