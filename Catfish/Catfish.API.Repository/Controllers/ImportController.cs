using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Import;
using Newtonsoft.Json.Serialization;

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.BackgroundJob.Root)]
    public class ImportController : ControllerBase
    {
        //private readonly RepoDbContext _context;
        private readonly IImportService _importService;
        public ImportController(IImportService importService)
        {
            _importService = importService;
            // _context = context;

        }
        // GET: api/Forms
        [HttpPost("from-excel")]
       public ActionResult FromExcel([FromForm] string value/*, [FromForm] IFormFile file */)
        {
            /* if (!ModelState.IsValid)
                 return BadRequest();

             var settings = new JsonSerializerSettings()
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                 TypeNameHandling = TypeNameHandling.All,
                 ContractResolver = new CamelCasePropertyNamesContractResolver()
             };*/
            // ExcelData data = JsonConvert.DeserializeObject<ExcelData>(value, settings);
            //Debug ONLY
            ExcelData data = new ExcelData();
            _importService.ImportFromExcel(data);

            return Ok();
        }
    }
}
