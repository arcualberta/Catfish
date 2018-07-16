using CommonServiceLocator;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Impl;
using System;
using System.Collections.Generic;

namespace Catfish.Core.Services
{
    public class SolrService
    {
        public static bool IsInitialized { get; private set; }

        private static SolrConnection mSolr { get; set; }

        public static void Init(string server)
        {
            IsInitialized = false;
            if (!string.IsNullOrEmpty(server))
            {
                mSolr = new SolrConnection(server);
                Startup.Init<SolrIndex>(mSolr);

                //TODO: Should we update the database here or have it in an external cron job

                IsInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("The app parameter Solr Server string has not been defined.");
            }
        }

        public static string EscapeQueryString(string searchString)
        {
            string result = searchString.Replace("\"", "\\\"")
                .Replace(":", "\\:");

            return new SolrQuery(result).Query;
        }

        public SolrService()
        {
        }

        public string GetGraphData(string query, string xIndexId, string yIndexId, string categoryId)
        {
            const string facetJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit : 1000,
                    field : {0},
                    facet : {{
                        sumYValues : ""sum({1})"",
                        groups : {{
                            type : terms,
                            field : {2},
                            limit: 10000,
                            facet : {{
                                sumYValuesArg : ""sum({1})""
                            }}
                        }}
                    }}
                }}
            }}";

            if (SolrService.IsInitialized)
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();

                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("json.facet", string.Format(facetJson, xIndexId, yIndexId, categoryId)),
                    new KeyValuePair<string, string>("rows", "0"),
                    new KeyValuePair<string, string>("sort", xIndexId + " asc"),
                    new KeyValuePair<string, string>("wt", "xml")
                };

                var result = SolrService.mSolr.Get("/select", parameters);

                return result;                
            }

            return null;
        }        
    }

    public class SolrIndex
    {
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }

        [SolrField("id_s")]
        public int Id { get; set; }
    }

    public class GraphData
    {
        
    }
}
