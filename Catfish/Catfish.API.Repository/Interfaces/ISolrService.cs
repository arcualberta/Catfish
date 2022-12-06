using Catfish.API.Repository.Solr;

namespace Catfish.API.Repository.Interfaces
{
    public interface ISolrService
    {
        public void Index(EntityData entityList,List<FormTemplate> forms);
        public void Index(IList<EntityData> entities, List<FormTemplate> forms);
        public void Index(List<SolrDoc> docs);
        public void Commit();
        public SearchResult Search(string searchText, int start, int maxRows, int maxHighlightsPerEntry = 1);
        public SearchResult Search(SearchFieldConstraint[] constraints, int start, int maxRows, int maxHighlightsPerEntry = 1);
        public SearchResult ExecuteSearch(string query, int start, int max, int maxHiglightSnippets);
    }
}
