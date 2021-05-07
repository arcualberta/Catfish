using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class SolrService : ISolrService
    {
        protected readonly IConfiguration _config;
        private readonly string _solrCoreUrl;

        private SearchResult _result;
        public SolrService(IConfiguration config)
        {
            _config = config;
            _solrCoreUrl = config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');
        }
        public void Index(Entity entity)
        {
            //XElement xml = GetSampleDoc();
            //AddUpdateAsync(xml);

            SolrDoc doc = new SolrDoc(entity);
            AddUpdateAsync(doc);
        }

        public void AddUpdateAsync(SolrDoc doc)
        {
            XElement payload = new XElement("add");
            payload.Add(doc.Root);

            _ = AddUpdateAsync(payload);
        }

        public void Index(IList<Entity> entities)
        {
            //XElement xml = GetSampleDoc();
            //AddUpdateAsync(xml);
            var docs = entities.Select(entity => new SolrDoc(entity)).ToList();
            Index(docs);
        }

        public void Index(List<SolrDoc> docs)
        {
            XElement payload = new XElement("add");
            foreach (var doc in docs)
                payload.Add(doc.Root);

            _ = AddUpdateAsync(payload);
        }

        public async Task AddUpdateAsync(XElement payload)
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var content = new StringContent(payload.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
            using var client = new HttpClient();
            using var httpResponse = await client.PostAsync(uri, content).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

        public void Commit()
        {
            _ = CommitAsync();
        }

        public async Task CommitAsync()
        {
            return;

            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var client = new HttpClient();
            using var httpResponse = await client.GetAsync(uri).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }


        /// <summary>
        /// Simple search
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public SearchResult Search(string searchText, int start, int maxRows, int maxHighlightsPerEntry = 1)
        {
            string query = "doc_type_ss:item";

            if (!string.IsNullOrEmpty(searchText))
            {
                string[] fieldNames = GetFieldNames();
                List<string> queryParams = new List<string>();
                foreach (var name in fieldNames)
                    queryParams.Add(string.Format("{0}:\"{1}\"", name, searchText));

                query = string.Join(" OR ", queryParams);
                query = string.Format("({0}) AND doc_type_ss:item", query);
            }

            _result = null;
            var task = ExecuteSearchQuery(query, start, maxRows, maxHighlightsPerEntry);
            task.Wait(60000);//Wait for a maximum of 1 minute
            return _result;
        }

        /// <summary>
        /// Advanced search
        /// </summary>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public SearchResult Search(SearchFieldConstraint[] constraints, int start, int maxRows, int maxHighlightsPerEntry = 1)
        {
            //Build the query by "and"ing all constraints and execute it.
            //Get the results and return them through the SearchResult object.

            List<string> queryParams = new List<string>();
            foreach (var constraint in constraints)
            {
                string solrFieldType = "ts";
                var fieldName = string.Format("{0}_{1}_{2}_{3}",
                    SearchFieldConstraint.ScopeStr(constraint.Scope),
                    constraint.ContainerId,
                    constraint.FieldId,
                    solrFieldType);

                queryParams.Add(string.Format("{0}:\"{1}\"", fieldName, constraint.SearchText));
            }
            queryParams.Add("doc_type_ss:item");
            string query = string.Join(" AND ", queryParams);

            _result = null;
            var task = ExecuteSearchQuery(query, start, maxRows, maxHighlightsPerEntry);
            task.Wait(60000);//Wait for a maximum of 1 minute
            return _result;
        }

        ////public async Task ExecuteSearch(string query)
        ////{
        ////    string queryUri = "http://localhost:8983/solr/resoundingculture/select?" + query + "&q=*%3A*";
        ////    using var client = new HttpClient();
        ////    using var httpResponse = await client.GetAsync(queryUri).ConfigureAwait(false);

        ////    httpResponse.EnsureSuccessStatusCode();
        ////}
        
        /// <summary>
        /// Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected async Task ExecuteSearchQuery(string query, int start, int max, int maxHiglightSnippets)
        {
            string queryUri = _solrCoreUrl + "/select?hl=on&q=" + query +
                string.Format("&start={0}&rows={1}&hl.fl=*&hl.snippets={2}&wt=xml", start, max, maxHiglightSnippets);

            //hl=on&q=apple&hl.fl=manu&fl=id,name,manu,cat
            using var client = new HttpClient();
            using var httpResponse = await client.GetAsync(new Uri(queryUri)).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            string response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            _result = new SearchResult(response);
            _result.ItemsPerPage = max;
        }

        protected string[] GetFieldNames(string[] acceptedFieldPrefixes = null)
        {
            string queryUri = _solrCoreUrl + "/select?q=*:*&wt=csv&rows=0&facet";
            if (acceptedFieldPrefixes == null)
                acceptedFieldPrefixes = new string[] { "data", "metadata" };

            //hl=on&q=apple&hl.fl=manu&fl=id,name,manu,cat
            using var client = new HttpClient();
            string[] fieldNames = null;
            using (var task = client.GetAsync(new Uri(queryUri)))
            {
                task.Wait(60000);
                var task2 = task.Result.Content.ReadAsStringAsync();
                task2.Wait(60000);
                fieldNames = task2.Result
                    .Split(",")
                    .Where(f => acceptedFieldPrefixes.Contains(f.Split("_")[0]))
                    .ToArray();
            };
            return fieldNames;
        }


    }
}
