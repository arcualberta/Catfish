using Catfish.API.Repository.Interfaces;

using Catfish.API.Repository.Models.Import;
using ExcelDataReader;
using System.Data;

namespace Catfish.API.Repository.Services
{
    public class ImportService : IImportService
    {
        public bool ImportFromExcel(ExcelData dataModel)
        {
           System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //DEBUG
            string folderRoot = "C:\\Projects\\Import";
            string sourceFile = Path.Combine(folderRoot, "all-data-groupize.xlsx");

            using (var stream = System.IO.File.Open(sourceFile, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
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

            return true;
        }
    } 
    
}
