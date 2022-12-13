using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;



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
        public async Task<SearchResult> Post([FromForm] string query, [FromForm] int offset = 0, [FromForm] int max = 100)
        {
            SearchResult solrSearchResult = null;
            try
            { 
               solrSearchResult = await _solr.ExecuteSearch(query, offset, max, 10);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return solrSearchResult;
        }


    }
}
