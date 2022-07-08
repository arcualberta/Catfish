using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Catfish.Areas.Manager.Controllers.Api
{
    [Route("manager/api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SysAdmin")] //TODO: remove this and implement authorization based on workflow.
    public class WorkflowController : Controller
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly ISubmissionService _submissionService;
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _appDb;
        private readonly IBackupService _backupService;
        public WorkflowController(AppDbContext db,
            IEntityTemplateService entityTemplateService,
            ISubmissionService submissionService,
            IWorkflowService workflowService,
            IBackupService backupService)
        {
            _entityTemplateService = entityTemplateService;
            _submissionService = submissionService;
            _workflowService = workflowService;
            _appDb = db;
            _backupService = backupService;
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
                string templateContentBeforeUpdate = template.Content;

                if (template == null)
                    throw new Exception("Template not found");

                FieldContainer form = null; ;
                if (data.DataItemId != Guid.Empty)
                    form = template.GetDataItem(data.DataItemId);
                else if (data.MetadataSetId != Guid.Empty)
                    form = template.MetadataSets.Where(ms => ms.Id == data.MetadataSetId).FirstOrDefault();

                //check if this textField is coming from a composite Field, if it's get the fild's Child-Template
                FieldContainer compForm = GetCompositeFormTemplate(template, data.DataItemId, data.TextFieldId.ToString());

                if ((compForm != null) && (compForm.Id == form.Id))
                    form = compForm;

                //DataItem dataItem = template.GetDataItem(data.DataItemId);
                if (form != null)
                {
                    var field = form.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();

                    if (data.FieldType.Contains("TableRow"))
                        field = ((Catfish.Core.Models.Contents.Fields.TableField)form.Fields[0]).TableHead.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();
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
                        {
                            //throw new NotImplementedException();
                            var tf = (field as TableField).TableHead.Fields.Where(f => f.Id == data.TextFieldId).FirstOrDefault();
                            if(tf != null)
                                tf.Name.UpdateValue(data.TextFieldId, data.TextValue, lang);
                        }
                            
                        else if (typeof(CompositeField).IsAssignableFrom(field.GetType()))
                            throw new NotImplementedException();
                        else if (typeof(TextField).IsAssignableFrom(field.GetType()))
                            (field as TextField).UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else if (typeof(MonolingualTextField).IsAssignableFrom(field.GetType()))
                            (field as MonolingualTextField).UpdateValue(data.TextFieldId, data.TextValue, lang);
                        else
                            throw new Exception("The element to be updated is not found within name, description, or other known field types.");

                        if(templateContentBeforeUpdate !=  template.Content)
                        {
                            //reload the template from the database and back it up before saving the changes
                            //back to the database.
                            _backupService.Backup(_entityTemplateService.GetTemplate(data.TemplateId));

                            _appDb.SaveChanges();
                           
                        }
                    }

                }


                result.Message = "Data has been changed successfully";
            }
            catch (Exception ex)
            {
                //throw ex;
                result.Message = ex.Message;
                //result.Success = false;
            }

            return result;
        }

        [Route("AddOptionText")]
        [HttpPost]
        public ApiResult AddOptionText([FromBody] ItemParam data)
        {
            ApiResult result = new ApiResult();
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(data.TemplateId);
                if (template == null)
                    throw new Exception("Template not found");

                FieldContainer form = null; ;
                if (data.DataItemId != Guid.Empty)
                    form = template.GetDataItem(data.DataItemId);
              
                if (form != null)
                {
                    var field = form.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();

                    if(field != null)
                    {
                        //Backing up the template before adding the option
                        _backupService.Backup(template);

                        (field as OptionsField).AddOption(data.TextValue,data.TextFieldId, data.Language);
                        _appDb.SaveChanges();
                        result.Message = "Option has been added successfully.";
                    }

                }
            }catch(Exception ex)
            {
                result.Message = ex.Message;
                //throw ex;
            }

             return result;
        }

        [Route("RemoveOptionText")]
        [HttpPost]
        public ApiResult RemoveOptionText([FromBody] ItemParam data)
        {
            ApiResult result = new ApiResult();
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(data.TemplateId);
                if (template == null)
                    throw new Exception("Template not found");

                FieldContainer form = null; ;
                if (data.DataItemId != Guid.Empty)
                    form = template.GetDataItem(data.DataItemId);

                if (form != null)
                {
                    var field = form.Fields.Where(f => f.Id == data.FieldId).FirstOrDefault();
                    if (field == null)
                        throw new Exception("Field not found.");

                    if (field != null)
                    {
                        Option option = (field as OptionsField).Options.Where(o => o.Id == data.TextFieldId).FirstOrDefault();
                        if (option != null)
                        {
                            var numGuidMatchs = Regex.Matches(template.Content, option.Id.ToString()).Count;

                            //template.Content.ind
                            if (numGuidMatchs > 1)
                                result.Message = "You can't delete this option field because it refers by other field(s).";
                            else
                            {
                                //Backing up the template before removing the option
                                _backupService.Backup(template);

                                (field as OptionsField).RemoveOption(option.Id);
                                _appDb.SaveChanges();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                result.Message = ex.Message;
            }

            return result;
        }

        private DataItem GetCompositeFormTemplate(EntityTemplate template,Guid dataItemId, string textFieldId)
        {
            DataItem compositeForm = null;

            foreach (DataItem d in template.DataContainer)
            {

                if (d.Fields.Any(f => (typeof(CompositeField).IsAssignableFrom(f.GetType()))))
                {
                    var cf = d.Fields.Where(f => f.Content.Contains(textFieldId)).FirstOrDefault();
                    if (typeof(CompositeField).IsAssignableFrom(cf.GetType()))
                    {
                        compositeForm = (cf as CompositeField).ChildTemplate;
                        break;
                    }
                }

            }

            return compositeForm;
        }

        ////private bool ReferByVisibleIf(EntityTemplate template, Guid optionFieldId)
        ////{
        ////    bool found = false;

        ////    foreach(DataItem dt in template.DataContainer)
        ////    {
        ////        foreach(var field in dt.Fields.Where(f=> f.VisibilityCondition !=null).ToList())
        ////        {
        ////            if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
        ////            {
        ////                foreach(Option opt in (field as OptionsField).Options.Where(v=> !string.IsNullOrWhiteSpace(v.VisibilityCondition.Value)).ToList())
        ////                {
        ////                    if (opt.VisibilityCondition.Value.Contains(field.Id.ToString()) || opt.VisibilityCondition.Value.Contains(optionFieldId.ToString()))
        ////                    {
        ////                        found = true;
        ////                        break;
        ////                    }
        ////                }
        ////            }
        ////            if (found)
        ////                break;
        ////            if ((field as BaseField).VisibilityCondition.Value.Contains(optionFieldId.ToString()))
        ////            {
        ////                found = true;
        ////                break;
        ////            }
                    
        ////        }
        ////        if (found)
        ////            break;
        ////    }
        ////    return found;
        ////}
        ////private bool ReferByRequiredIf(EntityTemplate template, Guid optionFieldId)
        ////{
        ////    bool found = false;

        ////    foreach (DataItem dt in template.DataContainer)
        ////    {
        ////        foreach (var field in dt.Fields.Where(f=> !string.IsNullOrWhiteSpace(f.RequiredCondition.Value)).ToList())
        ////        {
                    
        ////            if ((field as BaseField).RequiredCondition.Value.Contains(optionFieldId.ToString()))
        ////            {
        ////                found = true;
        ////                break;
        ////            }
                    
        ////        }
        ////    }
        ////    return found;
        ////}
        ////private bool ReferByExpressionValue(EntityTemplate template, Guid fieldId, Guid optionFieldId)
        ////{
        ////    bool found = false;

        ////    foreach (DataItem dt in template.DataContainer)
        ////    {
        ////        foreach (var field in dt.Fields.Where(f=> f.HasValueExpression).ToList())
        ////        {
                   
        ////            if ((field as BaseField).ValueExpression.Value.Contains(fieldId.ToString()) || (field as BaseField).ValueExpression.Value.Contains(optionFieldId.ToString()))
        ////            {
        ////                found = true;
        ////                break;
        ////            }
                    
        ////        }
        ////    }
        ////    return found;
        ////}
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
        public string FieldType{ get; set; }
    }

    
   

}
