using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using Hangfire;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.SolrSearch.Root)]
    public class SolrSearchController : ControllerBase
    {
        
        private readonly ISolrService _solr;

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
                throw ex;
            }

            return solrSearchResult;
        }

        [HttpPost("schedule-search-job")]
        public async Task<string> ScheduleSearchJob(
            [FromForm] string query,
            [FromForm] string email 
            
           /* string? filterQuery = null,
            string? sortBy = null,
            string? fieldList = null,
            int maxHiglightSnippets = 1*/)
        {
            SearchResult solrSearchResult = null;
            string parentJobId = "";
            try
            {
               parentJobId = BackgroundJob.Enqueue(() => _solr.SubmitSearchJob(query, out solrSearchResult));
               // BackgroundJob.ContinueJobWith(parentJobId, () => _solr.NotifyUser(requestLabel, email));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parentJobId;
        }
    }
}
