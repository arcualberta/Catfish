using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Import;
using Newtonsoft.Json.Serialization;

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Import.Root)]
    public class ImportController : ControllerBase
    {
        //private readonly RepoDbContext _context;
        private readonly IExcelFileProcessingService _importService;
        public ImportController(IExcelFileProcessingService importService)
        {
            _importService = importService;
            // _context = context;

        }

        [HttpPost("schema-from-excel")]
        public ActionResult SchemaFromExcel(string templateName, string primarySheetName,  IFormFile file)
        {
            try
            {
                _importService.ImportEntityTemplateSchema(templateName, primarySheetName, file);
            }
            catch (Exception ex)
            {

            }

            return Ok();
        }


        [HttpPost("data-from-excel")]
        public ActionResult DataFromExcel(Guid templateId, string pivotColumnName, IFormFile file )
        {
           
            _importService.ImportDataFromExcel(templateId, pivotColumnName, file);

            return Ok();
        }
    }
}
