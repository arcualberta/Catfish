using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Catfish.Areas.Manager.Controllers.Api
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class WorkflowController : Controller
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly ISubmissionService _submissionService;
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _appDb;
        public WorkflowController(AppDbContext db,
            IEntityTemplateService entityTemplateService,
            ISubmissionService submissionService,
           
            IWorkflowService workflowService)
        {
            _entityTemplateService = entityTemplateService;
            _submissionService = submissionService;
            _workflowService = workflowService;
            _appDb = db;
        }

        [Route("SaveText")]
        [HttpPost]
        public ApiResult SaveText([FromBody] ItemParam data)//([FromForm] Guid templateId, [FromForm] Guid dataItemId, [FromForm] Guid fieldId, [FromForm] Guid textId, [FromForm] string value)//
        {
            ApiResult result = new ApiResult();
            string lang = string.IsNullOrEmpty(data.Language) ? "en" : data.Language;

            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(data.TemplateId);
                if (template == null)
                    throw new Exception("Template not found");

                FieldContainer form = null; ;
                if (data.DataItemId != Guid.Empty)
                    form = template.GetDataItem(data.DataItemId);
                else if (data.MetadataSetId != Guid.Empty)
                    form = template.MetadataSets.Where(ms => ms.Id == data.MetadataSetId).FirstOrDefault();

                //DataItem dataItem = template.GetDataItem(data.DataItemId);
                if (form != null)
                {
                    var field = form.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();

                    if (field != null)
                    {
                        if (field.Name.Values.Any(v => v.Id == data.TextFieldId))
                            field.Name.UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else if (field.Description.Values.Any(v => v.Id == data.TextFieldId))
                            field.Description.UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                            (field as OptionsField).UpdateOption(data.TextFieldId, data.TextValue, lang);
                        else if (typeof(InfoSection).IsAssignableFrom(field.GetType()))
                            (field as InfoSection).Content.UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else if (typeof(TableField).IsAssignableFrom(field.GetType()))
                            throw new NotImplementedException();
                        else if (typeof(CompositeField).IsAssignableFrom(field.GetType()))
                            throw new NotImplementedException();
                        else if (typeof(TextField).IsAssignableFrom(field.GetType()))
                            (field as TextField).UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else if (typeof(MonolingualTextField).IsAssignableFrom(field.GetType()))
                            (field as MonolingualTextField).UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else
                            throw new Exception("The element to be updated is not found within name, description, or other known field types.");

                            _appDb.SaveChanges();
                    }

                }


                result.Message = "Data has been changed successfully";
            }
            catch (Exception ex)
            {
                throw ex;
                //result.Message = ex.Message;
                //result.Success = false;
            }

            return result;
        }


      
        
    }
    public class ItemParam
    {
        public Guid TemplateId { get; set; }
        public Guid DataItemId { get; set; }
        public Guid MetadataSetId { get; set; }
        public  Guid FieldId { get; set; }
        public Guid TextFieldId { get; set; }
        public string TextValue { get; set; }
        public string Language { get; set; }
    }

}
