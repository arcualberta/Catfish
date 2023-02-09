using Catfish.API.Repository.Models.Import;

namespace Catfish.API.Repository.Interfaces
{
    public interface IImportService
    {
        public bool ImportEntityTemplateSchema(string templateName, string primaryFormName, IFormFile file);
        public bool ImportFromExcel(IFormFile file);
        
    }
}
