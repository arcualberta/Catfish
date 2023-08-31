using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using CatfishExtensions.DTO;
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
        private readonly IEmailService _email;
        protected readonly IConfiguration _config;

        public SolrSearchController(ISolrService solrService, IEmailService email, IConfiguration config)
        {
            _solr = solrService;
            _email = email;
            _config = config;
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
            [FromForm] string email,
            [FromForm] string label)
        {
            SearchResult solrSearchResult = null;
            string parentJobId = "";
            try
            {
                string fileName = $@"querySearchResult_{label.Replace(" ","_").Trim()}.csv";
               

                 string solrCoreUrl = _config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');
                parentJobId = BackgroundJob.Enqueue(() => _solr.SubmitSearchJobAsync(query, fileName, solrCoreUrl));

               /* Email emailDto = new Email();
                emailDto.Subject = "Background Job";
                emailDto.ToRecipientEmail = new List<string> { email };
                emailDto.Body = $@"Your background is done. You could download your data : {filename} </a>";

                BackgroundJob.ContinueJobWith(parentJobId, () => _email.SendEmail(emailDto));*/
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parentJobId;
        }
    }
}
