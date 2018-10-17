﻿using CommonServiceLocator;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public class SolrService
    {
        public static bool IsInitialized { get; private set; }
        public static ISolrOperations<Dictionary<string, object>> solrOperations { get; set; }

        private static ISolrConnection mSolr { get; set; }
        private static bool IsSolrInitialized { get; set; } = false;
        

        public static void Init(string server)
        {
            IsInitialized = false;

            if (!string.IsNullOrEmpty(server))
            {
                ISolrConnection connection = new SolrConnection(server);

                SolrService.InitWithConnection(connection);
                SolrService.solrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<Dictionary<string, object>>>();
                IsInitialized = true;

            }
            else
            {
                throw new InvalidOperationException("The app parameter Solr Server string has not been defined.");
            }
        }

        public static void InitWithConnection(ISolrConnection connection)
        {
            if (!IsSolrInitialized)
            {
                mSolr = connection;
                Startup.Init<SolrIndex>(mSolr);
                Startup.Init<Dictionary<string, object>>(mSolr);
                IsSolrInitialized = true;
            }
            
            
            //TODO: Should we update the database here or have it in an external cron job

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
                    new KeyValuePair<string, string>("facet", "on"),
                    new KeyValuePair<string, string>("facet.field", field)
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

                if (result != null)
                {
                    SolrResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<SolrResponse>(result);

                    if (response.grouped.ContainsKey(fieldId))
                    {
                        foreach (var g in response.grouped[fieldId].groups)
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

        public IDictionary<string, StatsResult> GetStats(string field, string query/*, string groupByField= ""*/)
        {
            if (SolrService.IsInitialized)
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var results = solr.Query(query, new QueryOptions
                {
                    Rows = 0,
                    ExtraParams = new KeyValuePair<string, string>[]
                        {
                        new KeyValuePair<string,string>("stats", "true"),
                        new KeyValuePair<string, string>("stats.field", field)
                        }
                });
                return results.Stats;
            }
            return null;
        }

       // public IDictionary<string, ICollection<KeyValuePair<string, int>>> CountGroupBy(string field, string query, string groupByField)
        public string CountGroupBy(string field, string query, string groupByField)
        {
           
            const string facetJson = @"{{groupByCount : ""unique({0})""}}";

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("json.facet",string.Format(facetJson, groupByField)),
                    new KeyValuePair<string, string>("rows", "0")

                };

                var result = SolrService.mSolr.Get("/select", parameters);

                var xmlRes = XElement.Parse(result);
                XNode lastNode = xmlRes.LastNode;
                foreach (XNode n in (lastNode as XElement).Nodes().ToList())
                {
                    if ((n as XElement).Attribute("name").Value == "groupByCount")
                    {
                        var v = (n as XElement).Value;
                        return (n as XElement).Value;
                    }
                }

                return result;
            }
            return null;
        }

        public string GetMedian(string field, string query)
        {
            const string facetJson = @"{{Median : ""percentile({0},50)""}}";

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("json.facet",string.Format(facetJson, field)),
                    new KeyValuePair<string, string>("rows", "0")
                   
                };

                var result = SolrService.mSolr.Get("/select", parameters);

                 var xmlRes = XElement.Parse(result);
                XNode lastNode = xmlRes.LastNode;
                foreach(XNode n in (lastNode as XElement).Nodes().ToList())
                {
                    if((n as XElement).Attribute("name").Value == "Median")
                    {
                        var v = (n as XElement).Value;
                        return (n as XElement).Value;
                    }
                }

                return result;
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

        public decimal CountField(string field, string query = "*:*", string groupByField=null)
        {
            if (string.IsNullOrEmpty(groupByField))
            {
                var stats = GetStats(field, query);
                if (stats != null)
                {
                    return Convert.ToDecimal(stats[field].Count);
                }
            }            
            else
            {
                var count = CountGroupBy(field, query, groupByField);
                //groupByField is not null or empty
               if(count != null)
                    return Convert.ToDecimal(count);
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

        public decimal MedianField(string field, string query = "*:*")
        {
            //query = "percentile(50)";
            var median = GetMedian(field, query);

            if (median != null)
            {
                return Convert.ToDecimal(median);
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
