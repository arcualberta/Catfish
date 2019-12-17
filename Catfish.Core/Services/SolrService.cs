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
    /// <summary>
    /// Used to interact with a defined Solr instace.
    /// </summary>
    public class SolrService
    {
        /// <summary>
        /// States whether or not the service has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Global solr operations that are waiting to be comitterd for the current transaction.
        /// </summary>
        public static ISolrOperations<Dictionary<string, object>> SolrOperations { get; set; } = null;

        private static ISolrConnection mSolr { get; set; }
        private static bool IsSolrInitialized { get; set; } = false;

        /// <summary>
        /// The current solr time to wait for a timeout.
        /// </summary>
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
        
        /// <summary>
        /// Initializes the solr server. If one is already initialized, then it's closed and reinitialized.
        /// </summary>
        /// <param name="server">The server string to connect to.</param>
        public static void ForceInit(string server)
        {
            Init(server, true);
        }

        /// <summary>
        /// Initializes the solr server. If one is already initialized, then it's closed and reinitialized.
        /// </summary>
        /// <param name="connection">The server connection.</param>
        public static void ForceInit(ISolrConnection connection)
        {
            Init(connection, true);
        }

        /// <summary>
        /// Initializes the Solr service with a connection to solr.
        /// </summary>
        /// <param name="server">The server string to connect to.</param>
        /// <param name="force">If true, this will reinitialize the server connection if one already exists.</param>
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

        /// <summary>
        /// Initializes the Solr service with a connection to solr.
        /// </summary>
        /// <param name="connection">The solr connection to use.</param>
        /// <param name="force">If true, this will reinitialize the server connection if one already exists.</param>
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

        /// <summary>
        /// Obtains all strings that match the desired text in the specified field.
        /// </summary>
        /// <param name="field">The field to search.</param>
        /// <param name="text">The partial text to search on.</param>
        /// <param name="rows">The max number of rows to return.</param>
        /// <returns>A JSON result containing all matching strings.</returns>
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
                    new KeyValuePair<string, string>("facet.field", field),
                    new KeyValuePair<string, string>("facet.mincount", "1")
                };

                var result = SolrService.mSolr.Get("/select", parameters);

                return result;
            }

            return string.Empty;
        }

        /// <summary>
        /// Escapes the values of a query string so that it will work correctly in solr.
        /// </summary>
        /// <param name="searchString">the string to escape.</param>
        /// <returns>The resulting escaped string.</returns>
        public static string EscapeQueryString(string searchString)
        {
            string result = searchString.Replace("\"", "\\\"")
                .Replace(":", "\\:");

            return new SolrQuery(result).Query;
        }

        public SolrService()
        {
        }

        public string GetGraphData(string query, string xIndexId, string yIndexId, string categoryId, int itmLimit=10000)
        {
            const string facetCategoryJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit : {3},
                    field : {0},
                    facet : {{
                        sumYValues : {1},
                        groups : {{
                            type : terms,
                            field : {2},
                            limit:  {3},
                            facet : {{
                                sumYValuesArg : {1}
                            }}
                        }}
                    }}
                }}
            }}";

            const string facetJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit :  {3},
                    field : {0},
                    facet : {{
                        sumYValues : {1}
                    }}
                }}
            }}";

            

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("json.facet", string.Format(categoryId == null ? facetJson : facetCategoryJson, xIndexId, yIndexId, categoryId, itmLimit)),
                    new KeyValuePair<string, string>("rows", "0"),
                    new KeyValuePair<string, string>("sort", "field(" + xIndexId + ",min) asc"),
                    new KeyValuePair<string, string>("wt", "xml"),
                    new KeyValuePair<string, string>("fl", string.Join(",", xIndexId, yIndexId, categoryId)) // We can limit the search results to just the fields we will need for calculating the graphs.
                };

                var result = SolrService.mSolr.Get("/select", parameters);

                return result;
            }

            return null;
        }

        public IDictionary<string, string> GetSolrCategories(string query, string fieldId, int rows = int.MaxValue, bool isGraphData=true)
        {
            var dictionary = new Dictionary<string, string>();

            if (SolrService.IsInitialized)
            {
                IEnumerable<KeyValuePair<string, string>> parameters = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>("q", query),
                    new KeyValuePair<string, string>("rows", rows.ToString()),
                    new KeyValuePair<string, string>("wt", "json"),
                    new KeyValuePair<string, string>("facet.field", fieldId),
                    new KeyValuePair<string, string>("facet", "on"),
                    new KeyValuePair<string, string>("fl", fieldId),
                    new KeyValuePair<string, string>("sort", fieldId + " asc")
                };

                string result = mSolr.Get("/select", parameters);

                if (result != null)
                {
                    SolrResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<SolrResponse>(result);

                    if (response.facet_counts != null && response.facet_counts.facet_fields.ContainsKey(fieldId))
                    {
                       
                        foreach (var g in response.facet_counts.GetFacetsForField(fieldId))
                        {
                            if (isGraphData)
                            {
                                dictionary.Add(g.Item1, g.Item1);
                            }
                            else
                            {
                                dictionary.Add(g.Item1, g.Item2.ToString()); //MR: Nov12 2019: get the group by and count for each category
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
        private static readonly string[] FIELDS = { "id", "id_s" };
        
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }

        [SolrField("id_s")]
        public int Id { get; set; }

        public static ICollection<string> Fields
        {
            get
            {
                return FIELDS;
            }
        }
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
        public Dictionary<string, List<object>> facet_fields { get; set; }

        public List<Tuple<string, long>> GetFacetsForField(string fieldId)
        {
            List<Tuple<string, long>> result = new List<Tuple<string, long>>();

            if (facet_fields.ContainsKey(fieldId))
            {
                List<object> data = facet_fields[fieldId];

                for (int i = 0; i < data.Count; i += 2)
                {
                    result.Add(new Tuple<string, long>((string)data[i], (long)data[i + 1]));
                }
            }

            return result;
        }
    }
}
