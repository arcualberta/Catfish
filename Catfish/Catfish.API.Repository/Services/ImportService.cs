using Catfish.API.Repository.Interfaces;

using Catfish.API.Repository.Models.Import;
using ExcelDataReader;
using System.Data;

namespace Catfish.API.Repository.Services
{
    public class ImportService : IImportService
    {
        public bool ImportFromExcel(ExcelData dataModel, IFormFile file)
        {
           System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
           // var filePath = Path.GetTempFileName();
            //copy file
            /* using (var stream = System.IO.File.Create(filePath))
             {
                 await formFile.CopyToAsync(stream);
             }*/


            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                

                using (var excelDataReader = ExcelReaderFactory.CreateReader(stream))
                {
                    //  IExcelDataReader excelDataReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                    var conf = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = a => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    DataSet dataSet = excelDataReader.AsDataSet(conf);

                    DataRowCollection rows = dataSet.Tables["Attendees"].Rows;// dataSet.Tables[dataModel.PrimarySheet].Rows;//read all row in a sheet

                    for (int i = 0; i < rows.Count; i++)//foreach (DataRow row in rows)
                    {
                        var cols = rows[i].ItemArray; //get all cols in that row
                    }
                }
            }

            return true;
        }
    } 
    
}
