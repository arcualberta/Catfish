using Catfish.API.Repository.Models.Import;
using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IExcelFileProcessingService
    {
        public EntityTemplate ImportEntityTemplateSchema(string templateName, string primaryFormName, IFormFile file);
        public HttpStatusCode ImportDataFromExcel(Guid templateId, IFormFile file, string pivotColumnName);
        
    }
}
