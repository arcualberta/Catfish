using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using System.Text;
using System.Xml.Linq;

namespace Catfish.API.Repository.Services
{
    public class SolrService : ISolrService
    {
        protected readonly IConfiguration _config;
        private readonly string _solrCoreUrl;
        //private readonly ErrorLog _errorLog;
        private SearchResult _result;
        private readonly bool _indexFieldNames;
        public SolrService()
        {

        }

        public SolrService(IConfiguration config)
        {
            _config = config;
            _solrCoreUrl = config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');
            //_errorLog = errorLog;

            _indexFieldNames = false;
            _ = bool.TryParse(_config.GetSection("SolarConfiguration:IndexFieldNames").Value, out _indexFieldNames);
        }
        public async Task Index(EntityData entity, List<FormTemplate> forms)
        {
            SolrDoc doc = new SolrDoc(entity, forms, _indexFieldNames);
            await AddUpdateAsync(doc);
        }
        public async Task AddUpdateAsync(SolrDoc doc)
        {
            XElement payload = new XElement("add");
            payload.Add(doc.Root);

            await AddUpdateAsync(payload);
        }
        
        public async Task Index(IList<EntityData> entities, List<FormTemplate> forms)
        {
            var docs = entities.Select(entity => new SolrDoc(entity, forms, _indexFieldNames)).ToList();
            await Index(docs);
        }

        public async Task Index(List<SolrDoc> docs)
        {
            XElement payload = new XElement("add");
            foreach (var doc in docs)
                payload.Add(doc.Root);

            await AddUpdateAsync(payload); 
        }

        public async Task AddUpdateAsync(XElement payload)
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var content = new StringContent(payload.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
            using var client = new HttpClient();
            using var httpResponse = await client.PostAsync(uri, content).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task CommitAsync()
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var client = new HttpClient();
            using var httpResponse = await client.GetAsync(uri).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

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
        /// Executes a given valid solr query.
        /// </summary>Advanced search
        /// <param name="constraints"></param>
        /// <param name="start"></param>
        /// <param name="maxRows"></param>
        /// <param name="maxHighlightsPerEntry"></param>
        /// <returns></returns>
        public SearchResult Search(SearchFieldConstraint[] constraints, int start, int maxRows, int maxHighlightsPerEntry = 1)
        {
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
        /// <summary>
        /// Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="maxHiglightSnippets"></param>
        /// <returns></returns>
        public SearchResult ExecuteSearch(string query, int start, int max, int maxHiglightSnippets)
        {
            _result = null;
            var task = ExecuteSearchQuery(query, start, max, maxHiglightSnippets);
            task.Wait(60000);//Wait for a maximum of 1 minute
            return _result;
        }

        /// <summary>
        /// /Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="maxHiglightSnippets"></param>
        /// <returns></returns>
        public async Task ExecuteSearchQuery(string query, int start, int max, int maxHiglightSnippets)
        {
            try
            {
                string qUrl = _solrCoreUrl + "/select?hl=on";
                var parameters = new Dictionary<string, string>();
                parameters["q"] = query;
                parameters["start"] = start.ToString();
                parameters["rows"] = max.ToString();
                parameters["hl.fl"] = "*";
                parameters["hl.snippets"] = maxHiglightSnippets.ToString();
                parameters["wt"] = "xml";

                var httpClient = new HttpClient();
                var postResponse = await httpClient.PostAsync(new Uri(qUrl), new FormUrlEncodedContent(parameters));
                postResponse.EnsureSuccessStatusCode();
                var postContents = await postResponse.Content.ReadAsStringAsync();

                _result = new SearchResult(postContents);
                _result.ItemsPerPage = max;

                /*
                string queryUri = _solrCoreUrl + "/select?hl=on&q=" + query +
                  string.Format("&start={0}&rows={1}&hl.fl=*&hl.snippets={2}&wt=xml", start, max, maxHiglightSnippets);

                //hl=on&q=apple&hl.fl=manu&fl=id,name,manu,cat
                using var client = new HttpClient();
                using var httpResponse = await client.GetAsync(new Uri(queryUri)).ConfigureAwait(false);

                httpResponse.EnsureSuccessStatusCode();

                string response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                _result = new SearchResult(response, _errorLog);
                _result.ItemsPerPage = max;
                */
            }
            catch (Exception ex)
            {
               // _errorLog.Log(new Error(ex));
            }
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
