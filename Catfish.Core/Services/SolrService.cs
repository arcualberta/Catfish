using CommonServiceLocator;
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
        public static bool IsInitialized { get; private set; } = false;
        public static ISolrOperations<Dictionary<string, object>> SolrOperations { get; set; } = null;

        private static ISolrConnection mSolr { get; set; }
        private static bool IsSolrInitialized { get; set; } = false;

        public static int Timeout
        {
            get
            {
                if (mSolr == null)
                    return -1;

                return ((SolrConnection)mSolr).Timeout;
            }

            set
            {
                if (mSolr == null)
                    return;

                ((SolrConnection)mSolr).Timeout = value;
            }
        }
        

        public static void ForceInit(string server)
        {
            Init(server, true);
        }

        public static void ForceInit(ISolrConnection connection)
        {
            Init(connection, true);
        }

        public static void Init(string server, bool force = false)
        {
            //IsInitialized = false;

            if (!string.IsNullOrEmpty(server))
            {
                ISolrConnection connection = new SolrConnection(server);
                Init(connection, force);                
            }
            else
            {
                throw new InvalidOperationException("The app parameter Solr Server string has not been defined.");
            }
        }

        public static void Init(ISolrConnection connection, bool force = false)
        {            

            if (force)
            {
                ClearContainer();
            }

            if (!IsInitialized)
            {                
                mSolr = connection;
                Startup.Init<SolrIndex>(mSolr);
                Startup.Init<Dictionary<string, object>>(mSolr);
                SolrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<Dictionary<string, object>>>();
                IsInitialized = true;
                //IsSolrInitialized = true;

            }         
        }

        private static void ClearContainer()
        {
            Startup.Container.Clear();
            Startup.InitContainer();
            //IsSolrInitialized = false;
            IsInitialized = false;
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
            const string facetCategoryJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit : 10000,
                    field : {0},
                    facet : {{
                        sumYValues : ""sum(field({1},max))"",
                        groups : {{
                            type : terms,
                            field : {2},
                            limit: 10000,
                            facet : {{
                                sumYValuesArg : ""sum(field({1},max))""
                            }}
                        }}
                    }}
                }}
            }}";

            const string facetJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit : 10000,
                    field : {0},
                    facet : {{
                        sumYValues : ""sum(field({1},max))""
                    }}
                }}
            }}";

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("json.facet", string.Format(categoryId == null ? facetJson : facetCategoryJson, xIndexId, yIndexId, categoryId)),
                    new KeyValuePair<string, string>("rows", "0"),
                    new KeyValuePair<string, string>("sort", "field(" + xIndexId + ",min) asc"),
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
                    new KeyValuePair<string, string>("facet.field", fieldId),
                    new KeyValuePair<string, string>("fl", fieldId),
                    new KeyValuePair<string, string>("sort", fieldId + " asc")
                };

                string result = mSolr.Get("/select", parameters);

                if (result != null)
                {
                    SolrResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<SolrResponse>(result);

                    if (response.facet_counts != null && response.facet_counts.facet_fields.ContainsKey(fieldId))
                    {
                        foreach (var g in response.facet_counts.facet_fields[fieldId])
                        {
                            if (g.Length > 0)
                            {
                                dictionary.Add((string)g[0], fieldId);
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
            const string facetJson = @"{{Median : ""percentile(field({0},min),50)""}}";

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
        public SolrFacetCount facet_counts { get; set; }
        
    }

    public class SolrFacetCount
    {
        public Dictionary<string, List<object[]>> facet_fields { get; set; }
    }
}
