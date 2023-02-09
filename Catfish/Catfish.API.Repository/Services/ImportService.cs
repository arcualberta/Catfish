using Catfish.API.Repository.Interfaces;

using Catfish.API.Repository.Models.Import;
using ExcelDataReader;
using System.Data;

namespace Catfish.API.Repository.Services
{
    public class ImportService : IImportService
    {
        public bool ImportFromExcel(IFormFile file)
        {
           System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // var filePath = Path.GetTempFileName();
            //copy file
            /* using (var stream = System.IO.File.Create(filePath))
             {
                 await formFile.CopyToAsync(stream);
             }*/



            ExcelData dataModel = new ExcelData();


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

        public bool ImportEntityTemplateSchema(string templateName, string primaryFormName, IFormFile file)
        {
            //Reaad the excel file.

            //Create a new EntityTemplate. Name it after the given template name,

            //For each sheet in the excel file
            FormTemplate form = CreateFormTemplate();

            //if the form name is as same as the primaryFormName, please set it's root = true.

            //Add the new form to the newly created template

            //Add the new template to the database

            //Save changes

            
            throw new NotImplementedException();
        }

        private FormTemplate CreateFormTemplate(/* Data in an excel sheet, aloing with the sheet name ExcelSheet */)
        {
            //Create a new FormTemplate
            //Set the name to the name of the sheet (tab)

            //Load all columns in the given sheet.
            //For each column
                //Assume the column content is string
                //Create a new ShortAnswer field with the name of the column heading
                //Add the new Short Answer field to the form

            //Add the new form to the database table but do not save changes yet.

            throw new NotImplementedException();
        }

    }

}
