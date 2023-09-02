using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using ElmahCore;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.SolrSearch.Root)]
    public class SolrSearchController : ControllerBase
    {
        
        private readonly ISolrService _solr;
        private readonly ErrorLog _errorLog;
        public SolrSearchController(ISolrService solrService)
        {
            _solr = solrService;
        }


        // POST api/<SolrSearchController>
        [HttpPost]
        public async Task<SearchResult> Post(
            [FromForm] string query, 
            [FromForm] int offset = 0, 
            [FromForm] int max = 100,
            string? filterQuery = null,
            string? sortBy = null,
            string? fieldList = null,
            int maxHiglightSnippets = 1)
        {
            SearchResult solrSearchResult = null;
            try
            { 
               solrSearchResult = await _solr.ExecuteSearch(query, offset, max, filterQuery, sortBy, fieldList, maxHiglightSnippets);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }

#pragma warning disable CS8603 // Possible null reference return.
            return solrSearchResult;
#pragma warning restore CS8603 // Possible null reference return.
        }


    }
}
