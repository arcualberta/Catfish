using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using ElmahCore;
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
        public string ScheduleSearchJob(
            [FromForm] string query,
            [FromForm] string fieldList,
            [FromForm] string email,
            [FromForm] string label)
        {
            string parentJobId = "";
            try
            {
                string fileName = $@"querySearchResult_{label.Replace(" ","_").Trim()}_{Guid.NewGuid()}.csv";
               

                 string solrCoreUrl = _config.GetSection("SolarConfiguration:solrCore").Value.TrimEnd('/');
                parentJobId = BackgroundJob.Enqueue(() => _solr.SubmitSearchJobAsync(query, fieldList, fileName, solrCoreUrl));

                Email emailDto = new Email();
                emailDto.Subject = "Background Job";
                emailDto.ToRecipientEmail = new List<string> { email };
                emailDto.CcRecipientEmail = new List<string> { "arcrcg@ualberta.ca"};
                //https://localhost:5020/api/solr-search/get-file?fileName=querySearchResult_whole_data_set.csv
                string downloadLink = Request.Scheme + "://" + Request.Host.Value.TrimEnd('/') + Request.Path + "?fileName=" + fileName;
               

                emailDto.Body = $@"Your background is done. You could download your data :<a href='{downloadLink}' target='_blank'> {fileName} </a>";

                BackgroundJob.ContinueJobWith(parentJobId, () => _email.SendEmail(emailDto));
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
