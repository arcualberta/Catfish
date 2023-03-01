using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface ICsvFileProcessingService
    {
        public EntityTemplate ImportEntityTemplateSchema(string templateName,IFormFile file);
        public HttpStatusCode ImportDataFromExcel(Guid templateId, IFormFile file);

    }
}
