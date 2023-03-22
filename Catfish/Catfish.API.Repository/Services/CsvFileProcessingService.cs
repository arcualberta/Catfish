using Catfish.API.Repository.Interfaces;
using CatfishExtensions.DTO.Forms;
using System.Net;

namespace Catfish.API.Repository.Services
{
    public class CsvFileProcessingService : ICsvFileProcessingService
    {
        private readonly RepoDbContext _context;
        private readonly IEntityTemplateService _entityTemplateService;

        public CsvFileProcessingService(RepoDbContext context, IEntityTemplateService entityTemplateService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
        }

        public HttpStatusCode ImportDataFromExcel(Guid templateId, IFormFile file)
        {
            EntityTemplate entityTemplate = _entityTemplateService.GetEntityTemplate(templateId);
            if (entityTemplate == null)
                throw new CatfishException("Specified Entity Template not found", HttpStatusCode.NotFound);//return HttpStatusCode.NotFound;

            FormTemplate primaryFormTemplate = entityTemplate.Forms.Where(f => f.Id == entityTemplate!.EntityTemplateSettings!.PrimaryFormId).FirstOrDefault();

            if (primaryFormTemplate == null)
                throw new CatfishException("Specified Primary Form Template not found", HttpStatusCode.NotFound);

            string primarySheetName = primaryFormTemplate!.Name!;

            using (var stream = file.OpenReadStream())
            {

                using (var srReader = new StreamReader(stream))
                {

                   var headers = srReader!.ReadLine();//1st line
                    while (!srReader.EndOfStream)
                    {
                        string[] cols = srReader.ReadLine().Split(',');
                        

                        EntityData entityData = CreateEntityData(templateId, primaryFormTemplate.Id,  entityTemplate.Forms.ToList(), cols, eEntityType.Item);

                        //save to the db
                        _context!.Entities!.Add(entityData);

                    }

                }
            }
           
            return HttpStatusCode.OK;
        }

        public EntityTemplate ImportEntityTemplateSchema(string templateName, IFormFile file)
        {

            try
            {
                EntityTemplate template;

                template = CreateEntityTemplate(templateName, file);

                _context.EntityTemplates!.Add(template);
                _context.SaveChanges();
                return template;
            }
            catch (Exception ex)
            {
                throw new CatfishException(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private EntityTemplate CreateEntityTemplate(string templateName, IFormFile file)
        {
            //Create a new FormTemplate
            EntityTemplate template = new EntityTemplate();
            template.Id = Guid.NewGuid();
            template.Name = templateName;
            template.Description = "Entity template created from csv file";
            template.Created = DateTime.Now;
            template.Updated = DateTime.Now;
            template.State = eState.Draft;
            template.Forms = new List<FormTemplate>();

            // System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //DataRowCollection rows;
            // DataSet dataSet;
            EntityTemplateSettings templateSettings = new EntityTemplateSettings();
            templateSettings.DataForms = new FormEntry[] { };
            List<FormEntry> dataForms = new List<FormEntry>();
            List<FormTemplate> formTemplates = new List<FormTemplate>();


            using (var stream = file.OpenReadStream())
            {
               
                using (var srReader = new StreamReader(stream))
                {

                    string[] headers = srReader!.ReadLine().Split(",");
                   
                        //dt.Columns.Add(header);
                    FormTemplate formTemplate = CreateFormTemplate(headers, "Form Template Name");
                    FormEntry entry = new FormEntry();
                    entry.Id = formTemplate.Id;
                    entry.IsRequired =  true; //since it's only 1 file
                    entry.Name = formTemplate!.Name!;
                    entry.State = formTemplate.Status;
                    dataForms.Add(entry);

                    //STILL NEED FIELD MAPPING!!!!

                    templateSettings.TitleField = new FieldEntry() { FieldId = formTemplate.Fields![0].Id, FormId = formTemplate.Id };
                    templateSettings.DescriptionField = new FieldEntry() { FieldId = formTemplate.Fields![1].Id, FormId = formTemplate.Id };
                  
                    templateSettings.PrimaryFormId = formTemplate.Id;

                    formTemplate.EntityTemplates.Add(template);
                    _context.Forms!.Add(formTemplate);


                    formTemplates.Add(formTemplate);

                }
            }
             templateSettings.DataForms = dataForms.ToArray();

             template.EntityTemplateSettings = templateSettings;
            template.Forms = formTemplates;
            return template;
        }
        private FormTemplate CreateFormTemplate(string[] headers, string templateName)
        {
            FormTemplate template = new FormTemplate();
            template.Id = Guid.NewGuid();
            template.Name = templateName;
            template.Description = "This form template was created from csv file '" + templateName + "'";
            template.Created = DateTime.Now;
            template.Updated = DateTime.Now;

            IList<Field> fields = new List<Field>();
            foreach (string colValue in headers)
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

        private EntityData CreateEntityData(Guid templateId, Guid primaryFormId,  List<FormTemplate> forms, string[] colsData, eEntityType eEntityType)
        {
            EntityData entity = new EntityData();
            entity.Id = Guid.NewGuid();
            entity.TemplateId = templateId;
            entity.EntityType = eEntityType;

            FormTemplate primaryForm = forms.Where(f => f.Id == primaryFormId).FirstOrDefault();
           

            entity.Title = colsData[0].ToString();// 1st col
            entity.Description = colsData[1].ToString();//2nd col
            entity.State = eState.Active; //??
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            List<FormData> formData = new List<FormData>();

            //get primary form content
            FormData data = new FormData();
            data.FormId = primaryFormId; //primaryFormId
            data.Created = DateTime.Now;
            data.Updated = DateTime.Now;
            data.Id = Guid.NewGuid();


           
            data.FieldData = CreateFieldData(primaryForm!.Fields!.ToList(), colsData);
            formData.Add(data);

            entity.Data = formData;
            return entity;

        }
        private List<FieldData> CreateFieldData(List<Field> fields, string[] colsData)
        {
            List<FieldData> dataList = new List<FieldData>();

            if (fields.Count != colsData.Length)
                throw new Exception("The  length of fields in the template is not match the data columns");

            for (int i = 0; i < fields.Count; i++)
            {
                FieldData fldData = new FieldData();
                fldData.Id = Guid.NewGuid();
                fldData.FieldId = fields[i].Id;

                string colValue = colsData[i]!.ToString();
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
