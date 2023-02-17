using Catfish.API.Repository.Interfaces;

using Catfish.API.Repository.Models.Import;
using ExcelDataReader;
using System.Data;
using System.Net;
using System.Data.SqlClient;

namespace Catfish.API.Repository.Services
{
    public class ExcelFileProcessingService : IExcelFileProcessingService
    {
        private readonly RepoDbContext _context;
        private readonly IEntityTemplateService _entityTemplateService;

        public ExcelFileProcessingService(RepoDbContext context, IEntityTemplateService entityTemplateService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
        }
        public HttpStatusCode ImportDataFromExcel(Guid templateId, string pivotColumnName, IFormFile file)
        {
          
            EntityTemplate entityTemplate = _entityTemplateService.GetEntityTemplate(templateId);
            if (entityTemplate == null)
                return HttpStatusCode.NotFound;

            FormTemplate primaryFormTemplate = entityTemplate.Forms.Where(f => f.Id == entityTemplate!.EntityTemplateSettings!.PrimaryFormId).FirstOrDefault();

            if (primaryFormTemplate == null)
                return HttpStatusCode.NotFound;

            string primarySheetName = primaryFormTemplate!.Name!;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
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
                  
                  
                    DataRowCollection   rows = dataSet!.Tables[primarySheetName]!.Rows;

                    //Check if the number of fields in the templates is same with the number of column on the sheet
                    if (primaryFormTemplate.Fields!.Count != rows[0].ItemArray!.Count())
                        return HttpStatusCode.BadRequest;

                    for (int i = 0; i < rows.Count; i++)//foreach (DataRow row in rows)
                    {
                        // var cols = rows[i].ItemArray; //get all cols in that row
                        EntityData entityData = CreateEntityData(templateId,primaryFormTemplate.Id, dataSet, entityTemplate.Forms.ToList(), rows[i], eEntityType.Item, pivotColumnName);

                        //save to the db
                        _context!.Entities!.Add(entityData);


                        //DEBUG  ONLY!!!!
                     //   if (i == 1)
                       //     break;//ONLY PROCESS 2 ROWS
                    }
                  _context.SaveChanges();
                }
            }

            return HttpStatusCode.NotFound;
        }

        public EntityTemplate ImportEntityTemplateSchema(string templateName, string primaryFormName, IFormFile file)
        {
            try
            {
                EntityTemplate template;

                //Reaad the excel file.
                // DataSet dataSet = GetSheetData(file);

                //assuming the haeder is in 1st row
                template = CreateEntityTemplate(templateName, primaryFormName, file);

                _context.EntityTemplates!.Add(template);
                _context.SaveChanges();
                return template;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
           
        }

        private FormTemplate CreateFormTemplate(DataRow headerRow, string currentSheetName)
        {
            FormTemplate template= new FormTemplate();
            
            template.Id = Guid.NewGuid();
            template.Name = currentSheetName;
            template.Description = "This form template was created from excel sheet '" + currentSheetName + "'";
            template.Created = DateTime.Now;
            template.Updated = DateTime.Now;

            IList<Field> fields = new List<Field>();
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

                fields.Add(field);
            }

            template.Fields = fields;
            return template;
        }
        private EntityTemplate CreateEntityTemplate(string templateName,  string primarySheetName,IFormFile file)
        {
            //Create a new FormTemplate
            EntityTemplate template = new EntityTemplate();
            template.Id = Guid.NewGuid();
            template.Name = templateName;
            template.Description = "Entity template created from excel workbook";
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
                    for (int i = 0; i < excelDataReader.ResultsCount; i++)
                    {
                        var sheetName = excelDataReader.Name;

                        DataRowCollection rows = dataSet!.Tables[sheetName]!.Rows;

                        //assuming the 1st row is the header
                        FormTemplate formTemplate = CreateFormTemplate(rows[0], sheetName);

                        FormEntry entry = new FormEntry();
                        entry.Id = formTemplate.Id;
                        entry.IsRequired = sheetName.ToLower() == primarySheetName.ToLower() ? true : false;
                        entry.Name = formTemplate!.Name!;
                        entry.State = formTemplate.Status;
                        dataForms.Add(entry);

                        //STILL NEED FIELD MAPPING!!!!

                        templateSettings.TitleField = new FieldEntry() { FieldId = formTemplate.Fields![0].Id, FormId = formTemplate.Id };
                        templateSettings.DescriptionField = new FieldEntry() { FieldId = formTemplate.Fields![1].Id, FormId = formTemplate.Id };
                        if (formTemplate!.Name!.ToLower() == primarySheetName.ToLower())
                            templateSettings.PrimaryFormId = formTemplate.Id;

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

        private int GetPivotColumnIndex(DataSet data, string sheetName, string pivotColumn)
        {
            int? ind = data!.Tables[sheetName]!.Columns[pivotColumn] == null? -1 :  data!.Tables[sheetName]!.Columns[pivotColumn]?.Ordinal;

            return ind.Value; //data!.Tables[sheetName]!.Columns[pivotColumn]!.Ordinal; //get index of the pivotcolumn
        }
        
        private EntityData CreateEntityData(Guid templateId, Guid primaryFormId, DataSet dataSet, List<FormTemplate> forms, DataRow primaryRow, eEntityType eEntityType, string pivotColumn)
        {
            EntityData entity = new EntityData();
            entity.Id = Guid.NewGuid();
            entity.TemplateId = templateId;
            entity.EntityType = eEntityType;

            FormTemplate primaryForm = forms.Where(f => f.Id == primaryFormId).FirstOrDefault();
            int pivotColumIndex = GetPivotColumnIndex(dataSet, primaryForm!.Name!, pivotColumn);

            entity.Title = primaryRow!.ItemArray[0].ToString();// primaryRow!.ItemArray[pivotColumIndex]!.ToString();
            entity.Description = primaryRow!.ItemArray[pivotColumIndex].ToString();//pivot column should not be empty
            entity.State = eState.Active; //??
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            
            List<FormData> formData = new List<FormData>();
           
            //get primary form content
            FormData data = new FormData();
            data.FormId = primaryFormId; //primaryFormId
            data.Created = DateTime.Now;
            data.Updated=DateTime.Now;
            data.Id= Guid.NewGuid();

          
            string pivotColumnValue = "";
            pivotColumnValue = GetPivotColumnValue(primaryRow, primaryForm!.Name!, pivotColumIndex);
            data.FieldData = CreateFieldData(primaryForm!.Fields!.ToList(), primaryRow);
            formData.Add(data);
           

            //Get child form data
            foreach (FormTemplate form in forms)
            {
                if(form.Id != primaryFormId)
                { 
                    //Check if the sheet contain the pivot colum -- if not don't import the data
                    // if no pivot column found, it will return -1
                    pivotColumIndex = GetPivotColumnIndex(dataSet, form.Name!, pivotColumn);//have recheck in case the pivot column not in the same position in different sheet
                    
                    if (pivotColumIndex > -1)
                    {
                        var selectedRows = GetChildFormRows(dataSet, form.Name!, pivotColumIndex, pivotColumnValue);

                        foreach (DataRow row in selectedRows)
                        {
                            FormData dt = new FormData();
                            dt.FormId = form.Id;
                            dt.Created = DateTime.Now;
                            dt.Updated = DateTime.Now;
                            dt.Id = Guid.NewGuid();

                            dt.FieldData = CreateFieldData(form.Fields!.ToList(), row);

                            formData.Add(dt);
                        }
                    }
                }
            }

            entity.Data = formData;
            return entity;
           
        }

        private string GetPivotColumnValue(DataRow row, string sheetName, int pivotColumnIndex)
        {
            string val = "";
            val = row.ItemArray[pivotColumnIndex]!.ToString();

            return val;
        }
        private DataRow? GetChildFormRow(DataSet dataset, string sheetName,  string pivotColumnValue)
        {
            
            DataRowCollection rows = dataset.Tables[sheetName]!.Rows;
            object[] condition = new object[1];
            condition[0] = pivotColumnValue;

            DataRow selectedRow = rows.Find(condition[0]);
            return selectedRow;
        }

        private IEnumerable<DataRow> GetChildFormRows(DataSet dataset, string sheetName, int pivotColumIndex, string pivotColumnValue)
        { 

           DataRowCollection rows = (dataset.Tables[sheetName]!.Rows);
         
            var selectedRows = rows.Cast<DataRow>().Where(r => r[pivotColumIndex].ToString().ToLower() == pivotColumnValue.ToLower());
            return selectedRows;
        }
        private List<FieldData> CreateFieldData(List<Field> fields, DataRow row)
        {
            List<FieldData> dataList = new List<FieldData>();

            for( int i=0; i<fields.Count; i++)
            {
                FieldData fldData = new FieldData();
                fldData.Id = Guid.NewGuid();
                fldData.FieldId = fields[i].Id;

                string colValue = row!.ItemArray[i]!.ToString();
                TextCollection textCol = new TextCollection();
                textCol.Id = Guid.NewGuid();
                Text txtVal = new Text() { Id = Guid.NewGuid(), Lang = "en", Value = colValue, TextType = eTextType.ShortAnswer };
                textCol.Values = new Text[1];
                textCol.Values[0] = txtVal;
                fldData.MultilingualTextValues = new TextCollection[1];

                fldData.MultilingualTextValues[0] = textCol; 

                dataList.Add(fldData);
            }
            

            return dataList;
        }

    }

}
