using Catfish.API.Repository.Models.Import;

namespace Catfish.API.Repository.Interfaces
{
    public interface IImportService
    {
        public bool ImportFromExcel(ExcelData dataModel);
    }
}
