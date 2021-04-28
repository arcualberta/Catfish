﻿using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Helper;
using Microsoft.Extensions.Configuration;
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

            List<string> queryParams = new List<string>();
            foreach (var constraint in constraints)
            {
                string solrFieldType = "ss";
                var fieldName = string.Format("{0}_{1}_{2}_{3}",
                    SearchFieldConstraint.ScopeStr(constraint.Scope),
                    constraint.ContainerId,
                    constraint.FieldId,
                    solrFieldType);

                queryParams.Add(string.Format("{0}={1}", fieldName, constraint.SearchText));
            }

            string query = string.Join("&", queryParams);
            return ExecuteSearchQuery(query);

        }

        /// <summary>
        /// Executes a given valid solr query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected SearchResult ExecuteSearchQuery(string query)
        {
            string queryUri = "http://localhost:8983/solr/resoundingculture/select?" + query + "&q=*%3A*";
            throw new NotImplementedException();
        }

    }
}
