using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using ElmahCore;
using SolrNet;
using SolrNet.Commands.Parameters;
//using SolrNet.DSL;
using SolrNet.Mapping.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class QueryService : IQueryService
    {
        private readonly ISolrReadOnlyOperations<SolrEntry> _solrPageQuery;
        private readonly AppDbContext _db;
        private readonly ErrorLog _errorLog;
        public QueryService(ISolrReadOnlyOperations<SolrEntry> query, AppDbContext db, ErrorLog errorLog)
        {
            _solrPageQuery = query;
            _db = db;
            _errorLog = errorLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ISolrQuery BuildQuery(SearchParameters parameters)
        {
            try
            {
                var queryList = new List<ISolrQuery>();

                //Search for a given keyword in all configured Solr fields.
                queryList.Add(new SolrQueryByField("content", parameters.FreeSearch));


                return new SolrMultipleCriteriaQuery(queryList, "OR");
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IList<SolrEntry> FreeSearch(SearchParameters parameters, int start = 0, int limit = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parameters.FreeSearch))
                    return new List<SolrEntry>();

                var query = new SolrQuery("title_t:" + parameters.FreeSearch) +
                            new SolrQuery("content_t:" + parameters.FreeSearch);

                //Result hilighting: https://lucene.apache.org/solr/guide/8_5/highlighting.html
                string highlightStartTag = "<em class='bg-warning'>";
                string highlightEndTag = "</em>";

                var queryResult = _solrPageQuery.Query(query,
                    new QueryOptions
                    {
                        //Fields = new[] { "id", "title_ss", "object_type_i", "permalink_s", "containerId_ss" },
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

                List<SolrEntry> result = new List<SolrEntry>();
                for (int i = 0; i < queryResult.Count; ++i)
                {
                    var qr = queryResult[i];
                    var hkeys = highlights[i].Value.Keys.ToList();
                    var hvals = highlights[i].Value.Values.ToList();
                    for (int k = 0; k < hkeys.Count; ++k)
                    {
                        var snippets = hvals[k];
                        if (hkeys[k] == "title")
                        {
                            //The title filed matches the search criteria. In this
                            //case, we find the highlighted portion in the highlight
                            //and highlight the correcponding section in the actual title.

                            foreach (var snippet in snippets)
                                for (int t = 0; t < qr.Title.Count; ++t)
                                    qr.Title[t] = HighlightSections(snippet, highlightStartTag, highlightEndTag, qr.Title[t]);
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
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return new List<SolrEntry>();
            }
            
        }


        public IList<SolrEntry> KeywordSearch(string[] keywords, string[] categories, int start = 0, int limit = 100)
        {
            try
            {
                //If keywords or categories are not defined, we return no results.
                if (keywords.Length == 0 || categories.Length == 0)
                    return new List<SolrEntry>();

                AbstractSolrQuery keywordSubQuery = null;
                if (keywords.Contains("*"))
                    keywordSubQuery = new SolrHasValueQuery("keywords_ss");
                else
                {
                    foreach (var val in keywords)
                    {
                        if (keywordSubQuery == null)
                            keywordSubQuery = new SolrQuery("keywords_ss:" + val);
                        else
                            keywordSubQuery += new SolrQuery("keywords_ss:" + val);
                    }
                }

                AbstractSolrQuery categorySubQuery = null;
                if (categories.Contains("*"))
                    categorySubQuery = new SolrHasValueQuery("categories_ss");
                else
                {
                    foreach (var val in categories)
                    {
                        if (categorySubQuery == null)
                            categorySubQuery = new SolrQuery("categories_ss:" + val);
                        else
                            categorySubQuery += new SolrQuery("categories_ss:" + val);
                    }
                }

                AbstractSolrQuery query = keywordSubQuery && categorySubQuery;

                ////List<string> fieldsToRetrieve = new List<string>() { "id", "title", "object_type_i", "permalink_s", "containerId" };
                ////if (getfullContent)
                ////{
                ////    fieldsToRetrieve.Add("content_ss");
                ////    fieldsToRetrieve.Add("containerId_ss");

                ////    fieldsToRetrieve.Add("images_ss");
                ////    fieldsToRetrieve.Add("imageContainerId_ss");
                ////}

                var queryResult = _solrPageQuery.Query(query,
                    new QueryOptions
                    {
                        StartOrCursor = new StartOrCursor.Start(start),
                        Rows = limit
                    });

                return queryResult;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }

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
