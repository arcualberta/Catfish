using Catfish.Core.Models;
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
            string lang = "en";
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(data.TemplateId);
                if(template != null)
                {
                    DataItem dataItem = template.GetDataItem(data.DataItemId);
                    if (dataItem != null)
                    {
                        var field = dataItem.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();

                        if (field != null)
                        {
                            if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                            {
                                //DDL, RadioField and Checkboxes
                                if(field.Name.Values.Any(v=>v.Id == data.TextFieldId))
                                {
                                    field.SetName(data.TextValue, lang);
                                }
                                else
                                {
                                    foreach(Option opt in (field as OptionsField).Options)
                                    {
                                        if(opt.OptionText.Values.Any(v=> v.Id == data.TextFieldId))
                                        {
                                            opt.SetOptionText(data.TextValue, lang);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //TextField, TextArea, NumberField
                                field.SetName(data.TextValue, lang);
                            }



                            _appDb.SaveChanges();
                        }
                       
                    }
                }
               

                result.Message="Data has been changed successfully";
            }
            catch (Exception ex)
            {
               // throw ex;
                result.Message=ex.Message;
            }

            return result;
        }


      
        
    }
    public class ItemParam
    {
        public Guid TemplateId { get; set; }
        public Guid DataItemId { get; set; }
        public  Guid FieldId { get; set; }
        public Guid TextFieldId { get; set; }
        public string TextValue { get; set; }
    }

}
