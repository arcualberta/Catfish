using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using CommonServiceLocator;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void GetGraphData(string query, string xIndexId, string yIndexId, string categoryId)
        {
            const string facetJson = @"{{
                xValues:{{
                    sort : index,
                    type : terms,
                    limit : 1000,
                    field : {0},
                    facet : {{
                        sumXValues : ""sum({1})"",
                        groups : {{
                            type : terms,
                            field : {2},
                            facet : {{
                                sumXValuesArg : ""sum({1})""
                            }}
                        }}
                    }}
                }}
            }}";

            if (SolrService.IsInitialized)
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var test = System.Text.RegularExpressions.Regex.Replace(string.Format(facetJson, xIndexId, yIndexId, categoryId), @"\t|\n|\r", "");

                QueryOptions queryOptions = new QueryOptions
                {
                    Rows = 0,
                    ExtraParams = new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("json.facet", string.Format(facetJson, xIndexId, yIndexId, categoryId))
                    }
                };

                var result = solr.Query(query, queryOptions);

                var re = result.FirstOrDefault();
            }
        }
    }

    public class SolrIndex
    {
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }

        [SolrField("id_s")]
        public int Id { get; set; }
    }

    public class SolrAdvancedIndex
    {
        [SolrUniqueKey("id")]
        public string SolrId { get; set; }

        [SolrField("id_s")]
        public int Id { get; set; }

        [SolrField("value_")]
        public IDictionary<string, string> Values { get; set; }
    }
}
