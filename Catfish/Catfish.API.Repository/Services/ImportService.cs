using Catfish.API.Repository.Interfaces;

using Catfish.API.Repository.Models.Import;
using ExcelDataReader;
using System.Data;

namespace Catfish.API.Repository.Services
{
    public class ImportService : IImportService
    {
        private readonly RepoDbContext _context;

        public ImportService(RepoDbContext context)
        {
            _context = context;
        }
        public bool ImportFromExcel(IFormFile file)
        {
           System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
           

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

        public EntityTemplate ImportEntityTemplateSchema(string templateName, string primaryFormName, IFormFile file)
        {
            EntityTemplate template;
            if (!string.IsNullOrEmpty(templateName))
            {
                template = _context.EntityTemplates.Where(f => f.Name == templateName).FirstOrDefault();
                if (template != null)
                    return template;
            }

            //Reaad the excel file.
           // DataSet dataSet = GetSheetData(file);

            //assuming the haeder is in 1st row
           template = CreateEntityTemplate(templateName, primaryFormName, file);

            _context.EntityTemplates!.Add(template);
            _context.SaveChanges();
            return template;
           
        }

        private FormTemplate CreateFormTemplate(DataRow headerRow, string currentSheetName, string primarySheetName)
        {
            FormTemplate template= new FormTemplate();
            
            template.Id = Guid.NewGuid();
            template.Name = currentSheetName;
            template.Description = "This form template was created from excel sheet '" + currentSheetName + "'";
            template.Created = DateTime.Now;
            template.Updated = DateTime.Now;

          // EntityTemplateSettings templateSettings = new EntityTemplateSettings();

            //if (currentSheetName == primarySheetName) {
           //     template.isRe
           // }
            template.Fields = new List<Field>();
            List<Field> fields = new List<Field>();
            foreach(string colValue in headerRow.ItemArray.ToList())
            {
                Field field = new Field();
                field.Id = Guid.NewGuid();
                field.Type = FieldType.ShortAnswer;
                TextCollection textCol = new TextCollection();
                textCol.Id = Guid.NewGuid();
                Text txtVal = new Text() { Id = Guid.NewGuid(), Lang = "en", Value = colValue, TextType = eTextType.ShortAnswer };
                textCol.Values = new Text[1];
                textCol.Values[0] = txtVal;
                field.Title = textCol;

                //template.Fields.Add(field);
                fields.Add(field);
            }

            template.SerializedFields= JsonConvert.SerializeObject(fields);

          //  _context.Forms!.Add(template);
            return template;

        }
        private EntityTemplate CreateEntityTemplate(string templateName,  string primarySheetName,IFormFile file)
        {
            //Create a new FormTemplate
            EntityTemplate template = new EntityTemplate();
            template.Id = Guid.NewGuid();
            template.Name = templateName;
            template.Created = DateTime.Now;
            template.Updated = DateTime.Now;
            template.State = eState.Draft;
            template.Forms = new List<FormTemplate>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //DataRowCollection rows;
            DataSet dataSet;
            EntityTemplateSettings templateSettings = new EntityTemplateSettings();
            templateSettings.DataForms = new FormEntry[] { };
            List<FormEntry> dataForms = new List<FormEntry>();
            List<FormTemplate> formTemplates = new List<FormTemplate>();

            
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;


                using (var excelDataReader = ExcelReaderFactory.CreateReader(stream))
                {
                    var conf = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = a => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false //need the header
                        }
                    };
                    dataSet = excelDataReader.AsDataSet(conf);
                   for(int i=0; i<excelDataReader.ResultsCount; i++)
                    {
                        var sheetName = excelDataReader.Name;


                        DataRowCollection rows = dataSet.Tables[sheetName].Rows;

                        //assuming the 1st row is the header
                        FormTemplate formTemplate = CreateFormTemplate(rows[0], sheetName, primarySheetName);

                        FormEntry entry = new FormEntry();
                        entry.Id = formTemplate.Id;
                        entry.IsRequired = sheetName.ToLower() == primarySheetName.ToLower() ? true : false;
                        entry.Name = formTemplate.Name;
                        entry.State = formTemplate.Status;
                        dataForms.Add(entry);
                        
                        //STILL NEED FIELD MAPPING!!!!

                        templateSettings.TitleField = new FieldEntry() { FieldId = formTemplate.Fields![0].Id, FormId = formTemplate.Id };
                        templateSettings.DescriptionField = new FieldEntry() { FieldId = formTemplate.Fields![1].Id, FormId = formTemplate.Id };

                        formTemplate.EntityTemplates.Add(template);
                        _context.Forms!.Add(formTemplate);

                        formTemplates.Add(formTemplate);


                       
                        //read next sheet
                        excelDataReader.NextResult();


                    } 
                }
            }
            templateSettings.DataForms= dataForms.ToArray();

            template.EntityTemplateSettings = templateSettings;
            template.Forms= formTemplates;
            return template;
        }

    }

}
