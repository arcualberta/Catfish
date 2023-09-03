﻿using Catfish.API.Repository.Solr;
using System.Xml.Linq;

namespace Catfish.API.Repository.Interfaces
{
    public interface ISolrService
    {
        public Task Index(EntityData entityList,List<FormTemplate> forms);
        public Task Index(IList<EntityData> entities, List<FormTemplate> forms);
        public Task Index(List<SolrDoc> docs);
        public Task AddUpdateAsync(XElement payload);
        public Task AddUpdateAsync(string payloadXmlString);
        public Task CommitAsync();
        public Task<SearchResult> Search(string searchText, int start, int maxRows, int maxHighlightsPerEntry = 1);
        public Task<SearchResult> Search(SearchFieldConstraint[] constraints, int start, int maxRows, int maxHighlightsPerEntry = 1);
        public Task<SearchResult> ExecuteSearch(
            string query, 
            int start, 
            int max, 
            string? filterQuery = null, 
            string? sortBy = null, 
            string? fieldList = null,
            int maxHiglightSnippets = 1, 
            bool useSolrJson = false);

        public Task<string> ExecuteSolrSearch(string solrCoreUrl,
           string query,
           int start,
           int max,
           string? filterQuery = null,
           string? sortBy = null,
           string? fieldList = null,
           int maxHiglightSnippets = 1,
           string outputFormat = "csv");
        public Task SubmitSearchJobAsync(string query, string? fieldList, string notificationEmaill, string jobLabel, string solrCoreUrl, string downloadEndpoint, int batchSize, int maxRows, string fromEmail, string smtpServer, int smtpPort, bool ssl);

        public Task<int> GetMatchCount(string query, string solrCoreUrl="");

    }
}
