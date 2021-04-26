using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Helper;
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
        private readonly string _solrCoreUrl;
        public SolrService(ICatfishAppConfiguration config)
        {
            _solrCoreUrl = config.GetSolrCoreUrl().TrimEnd('/');
        }
        public void Index(Entity entity)
        {
            //XElement xml = GetSampleDoc();
            //AddUpdateAsync(xml);

            SolrDoc doc = new SolrDoc(entity);
            AddUpdateAsync(doc);
        }

        public void Commit()
        {
            _ = CommitAsync();
        }

        public void AddUpdateAsync(SolrDoc doc)
        {
            XElement payload = new XElement("add");
            payload.Add(doc.Root);

            _ = AddUpdateAsync(payload);
        }

        public void AddUpdateAsync(List<SolrDoc> docs)
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

        public async Task CommitAsync()
        {
            return;

            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var client = new HttpClient();
            using var httpResponse = await client.GetAsync(uri).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }


        ////private XElement GetSampleDoc()
        ////{
        ////    var id = Guid.NewGuid().ToString();
        ////    return XElement.Parse(string.Format(@"
        ////    <add><doc><field name='id'>{0}</field><field name='name_s'>change.me {1}</field></doc></add>
        ////        ", id, DateTime.Now.ToString()));

        ////}


        /// <summary>
        /// Simple search
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public SearchResult Search(string searchText)
        {
            string query = "";
            return ExecuteSearchQuery(query);
        }

        /// <summary>
        /// Advanced search
        /// </summary>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public SearchResult Search(SearchFieldConstraint[] constraints, int offset, int pageSize)
        {
            //Build the query by "and"ing all constraints and execute it.
            //Get the results and return them through the SearchResult object.

            string query = "";
            return ExecuteSearchQuery(query);

        }

        /// <summary>
        /// Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected SearchResult ExecuteSearchQuery(string query)
        {
            throw new NotImplementedException();
        }

    }
}
