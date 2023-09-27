using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using ElmahCore;
using Hangfire;
using CatfishExtensions.Services;
using Catfish.API.Repository.Models.BackgroundJobs;


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
            [FromForm] string? fieldList = null,
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

        [HttpPost("schedule-search-job")]
        public async Task<string> ScheduleSearchJob(
            [FromForm] string query,
            [FromForm] string? email,
            [FromForm] string label,
            [FromForm] int batchSize = 10000,
            [FromForm] string? fieldList = null,
            [FromForm] bool? selectUniqueEntries = false,
            [FromForm] int? numDecimalPoints = null,
            [FromForm] string? frequencyArrayFields = null,
            [FromForm] string? exportFields = null,
            [FromForm] string? user = null)
        {
            string parentJobId = "";
            try
            {
                string fileName = $@"querySearchResult_{label.Replace(" ","_").Trim()}_{Guid.NewGuid()}.csv";

                int matchCount = await _solr.GetMatchCount(query);
                string solrCoreUrl = _config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');

                string path = (Request.PathBase.Value?.Length > 0 ? Request.PathBase.Value : "")
                    + Request.Path.Value.Substring(0, Request.Path.Value.LastIndexOf("/")) + "/get-file";
                string downloadEndpoint = Request.Scheme + "://" + Request.Host.Value.TrimEnd('/') + path;

                JobRecord jobRecord = await _solr.CreateJobRecord(label, matchCount, user);
                

                parentJobId = BackgroundJob.Enqueue<ISolrService>((solrService) => solrService.SubmitSearchJobAsync(query, fieldList, email, jobRecord.Id, solrCoreUrl, downloadEndpoint, batchSize,/* matchCount,*/ selectUniqueEntries, numDecimalPoints, frequencyArrayFields, exportFields));

                await _solr.UpdateJobRecordHangfireId(jobRecord.Id, parentJobId);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parentJobId;
        }

        [HttpGet("get-file")]
        public FileContentResult GetFile(string fileName)
        {
            if (!fileName.Contains(".csv"))
                fileName = fileName + ".csv";

            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            string filePath = Path.Combine(uploadFolder, fileName!);
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException();

            string mimeType = "application/octet-stream";
            byte[] fileBytes;
            fileBytes = System.IO.File.ReadAllBytes(filePath);
            return new FileContentResult(fileBytes!, mimeType)
            {
                FileDownloadName = fileName
            };
        }

    }
}
