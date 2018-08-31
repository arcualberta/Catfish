using CommonServiceLocator;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;
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
                Startup.Init<Dictionary<string, object>>(mSolr);

                //TODO: Should we update the database here or have it in an external cron job

                IsInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("The app parameter Solr Server string has not been defined.");
            }
        }

        public static string GetPartialMatichingText(string field, string text, int rows = 10)
        {
            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", field + ":" + SolrService.EscapeQueryString(text + "*")),
                    new KeyValuePair<string, string>("rows", rows.ToString()),
                    new KeyValuePair<string, string>("sort", field + " asc"),
                    new KeyValuePair<string, string>("fl", field),
                    new KeyValuePair<string, string>("wt", "json"),
                    new KeyValuePair<string, string>("group", "true"),
                    new KeyValuePair<string, string>("group.field", field)
                };

                var result = SolrService.mSolr.Get("/select", parameters);

                return result;
            }

            return string.Empty;
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
                    limit : 10000,
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

        public IDictionary<string, string> GetSolrCategories(string query, string fieldId, int rows = int.MaxValue)
        {
            var dictionary = new Dictionary<string, string>();

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("rows", rows.ToString()),
                    new KeyValuePair<string, string>("wt", "json"),
                    new KeyValuePair<string, string>("group", "true"),
                    new KeyValuePair<string, string>("group.field", fieldId),
                    new KeyValuePair<string, string>("fl", fieldId)
                };

                string result = mSolr.Get("/select", parameters);

                if(result != null)
                {
                    SolrResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<SolrResponse>(result);

                    if (response.grouped.ContainsKey(fieldId))
                    {
                        foreach(var g in response.grouped[fieldId].groups)
                        {
                            if (g.groupValue != null)
                            {
                                dictionary.Add(g.groupValue, g.docList.docs[0][fieldId]);
                            }
                        }
                    }
                }
            }

            return dictionary;
        }

        public IDictionary<string, StatsResult> GetStats(string field, string query)
        {
            if (SolrService.IsInitialized)
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var results = solr.Query(query, new QueryOptions {
                    Rows = 0, ExtraParams = new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string,string>("stats", "true"),
                        new KeyValuePair<string, string>("stats.field", field)
                    }
                });

                return results.Stats;
            }

            return null;
        }
        
        public decimal SumField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if(stats != null && stats[field].Count > 0)
            {
                return Convert.ToDecimal(stats[field].Sum);
            }

            return 0m;
        }

        public decimal CountField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if (stats != null)
            {
                return Convert.ToDecimal(stats[field].Count);
            }

            return 0m;
        }

        public decimal MeanField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if (stats != null && stats[field].Count > 0)
            {
                return Convert.ToDecimal(stats[field].Mean);
            }

            return 0m;
        }

        public decimal MinField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if (stats != null && stats[field].Count > 0)
            {
                return Convert.ToDecimal(stats[field].Min);
            }

            return 0m;
        }

        public decimal MaxField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if (stats != null && stats[field].Count > 0)
            {
                return Convert.ToDecimal(stats[field].Max);
            }

            return 0m;
        }

        public decimal StandardDeviationField(string field, string query = "*:*")
        {
            var stats = GetStats(field, query);

            if(stats != null && stats[field].Count > 0)
            {
                return Convert.ToDecimal(stats[field].StdDev);
            }

            return 0m;
        }
    }

    public class SolrIndex
    {
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }

        [SolrField("id_s")]
        public int Id { get; set; }
    }

    public class DocList
    {
        public int numFound { get; set; }
        public IList<IDictionary<string, string>> docs { get; set; }
    }

    public class GroupedEntry
    {
        public string groupValue { get; set; }

        public DocList docList { get; set; }
    }

    public class GroupedResult
    {
        public IEnumerable<GroupedEntry> groups { get; set; }
    }

    public class SolrResponse
    {
        public Dictionary<string, GroupedResult> grouped { get; set; }
    }
}
