using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Piranha.AspNetCore.Identity.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly ISubmissionService _submissionService;
        private readonly IWorkflowService _workflowService;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _dotnetAuthorizationService;

        private readonly AppDbContext _appDb;
        private readonly IJobService _jobService;

        public ItemsController(AppDbContext db, 
            IEntityTemplateService entityTemplateService, 
            ISubmissionService submissionService, 
            IJobService jobService, 
            IConfiguration configuration, 
            IWorkflowService workflowService,
            Microsoft.AspNetCore.Authorization.IAuthorizationService dotnetAuthorizationService)
        {
            _entityTemplateService = entityTemplateService;
            _submissionService = submissionService;
            _workflowService = workflowService;
            _appDb = db;
            _jobService = jobService;
            _dotnetAuthorizationService = dotnetAuthorizationService;

            ConfigHelper.Configuration = configuration;
        }
        // GET: api/<ItemController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ItemController>/5
        /// <summary>
        /// get Items and only return the form fields (cuatom from form), the submitted form and the form status
        /// </summary>
        /// <param name="templateId">Item template id</param>
        /// <param name="collectionId">main collection Id</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">end date</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string GetItemList(Guid templateId, Guid? collectionId, DateTime? startDate, DateTime? endDate, Guid? reportTemplate)
        {
            //Making sure the startDate is trimmed to the begining of the day and the endDate is bumped up to the end of the day
            if (startDate.HasValue)
                startDate = startDate.Value.Date;
            if (endDate.HasValue)
                endDate = endDate.Value.Date.AddDays(1);

            EntityTemplate template = _entityTemplateService.GetTemplate(templateId);

            string errorMessage = "";
            string resultString = "";

            if (template == null)
                errorMessage = "No template found.";
            else
            {
                Core.Models.Contents.Reports.BaseReport selectedReport = template.Reports.Where(r => r.Id == reportTemplate).FirstOrDefault();

                XElement result = new XElement("table");
                result.SetAttributeValue("class", "table");

                XElement thead = new XElement("thead");
                result.Add(thead);

                XElement headRow = new XElement("tr");
                thead.Add(headRow);

                DataItem root = template.GetRootDataItem(false);
                if (root == null)
                    errorMessage = "No form found in the template.";
                else
                {
                    var fieldList = root.GetValueFields(); //MR: this only get regular field -- no composite fields
                    List<Item> itemList = _submissionService.GetSubmissionList(User, templateId, collectionId, startDate, endDate);
                   

                    headRow.Add(XElement.Parse("<th></th>"));

                    headRow.Add(XElement.Parse("<th>Submission Date</th>"));

                    List<Guid> selectedFieldGuids = new List<Guid>();
                    List<Guid> selectedCompositeFieldGuids = new List<Guid>();
                    foreach (var field in fieldList)
                    {
                        //MR March : 15 2021: only include field that selected on the Report schema
                        if (selectedReport != null)
                        {
                            foreach(var f in selectedReport.Fields)
                            {
                                if (f.FieldId == field.Id)
                                {
                                    //Display the FieldLabel/ColLabel if defined, otherwise display the Label of the field from the Form
                                    string colLabel = string.IsNullOrWhiteSpace(f.FieldLabel) ? field.Name.GetConcatenatedContent(" | ") : f.FieldLabel;
                                    //headRow.Add(XElement.Parse(string.Format("<th>{0}</th>", field.Name.GetConcatenatedContent(" | "))));
                                    headRow.Add(XElement.Parse(string.Format("<th>{0}</th>", colLabel)));
                                    selectedFieldGuids.Add(field.Id);
                                    break;
                                }
                            }
                        }
                        else
                        {  ///MR March : 15 2021: include all Fields if no Report schema existed
                            headRow.Add(XElement.Parse(string.Format("<th>{0}</th>", field.Name.GetConcatenatedContent(" | "))));
                            selectedFieldGuids.Add(field.Id);
                        }
                    }

                    //
                    //if composite Field
                    if (selectedReport != null)
                    {
                        foreach (var f in selectedReport.Fields)
                        {
                            if (f.ParentFieldId != null) //(f.ParentFieldId != null && f.ParentFieldId == field.Id)
                            {
                                //string flName = field.Name.GetConcatenatedContent(" | ");
                                 headRow.Add(XElement.Parse(string.Format("<th>{0}</th>", f.FieldLabel)));//to do
                                selectedCompositeFieldGuids.Add(f.FieldId);
                            }
                        }
                    }

                    headRow.Add(XElement.Parse("<th>Status</th>"));

                    XElement tbody = new XElement("tbody");
                    result.Add(tbody);

                   // var fieldGuids = fieldList.Select(field => field.Id).ToList();

                      
                    //Arrays to store already loaded status values instead of having to load them repeatedly from the database
                    List<Guid?> statusIds = new List<Guid?>();
                    List<string> statusVals = new List<string>();

                    if (itemList.Count == 0)
                        errorMessage = "No data found.";
                    else
                    {
                        foreach (Item item in itemList)
                        {
                            XElement bodyRow = new XElement("tr");
                            tbody.Add(bodyRow);

                            //TODO: check if the currently logged in user to perform the following actions
                            bool viewPermitted = true;
                            string viewLink = viewPermitted ? string.Format("<a href='/items/{0}' class='fa fa-eye' target='_blank'></a>", item.Id) : "";
                            bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", viewLink)));

                            bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", item.Created.ToString("yyyy-MM-dd"))));

                            DataItem dataItem = item.GetRootDataItem(false);

                            // List<string> fieldValues = dataItem.GetConcatenatedFieldValues(fieldGuids, " |");
                            List<string> fieldValues = dataItem.GetConcatenatedFieldValues(selectedFieldGuids, " |");

                            //if composite field involved -- get the value from associated item??

                            foreach (var val in fieldValues) //MR: These are just regular Field
                            {
                                //Replacing "&" characters with " and ";
                                var sanitizedVal = val.Replace("&", " and ");
                                bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", sanitizedVal)));
                            }

                            //MR: March 15 2021: -- get composite field values if any define in the Report
                            var compositeFields = item.DataContainer.Where(d => d.Fields.Any(f => f.GetType() == typeof(CompositeField) && ((CompositeField)f).Children.Count >= 1)).ToList();

                            foreach (var cf in compositeFields)
                            {

                                foreach (var f in cf.Fields)
                                {
                                    if (typeof(CompositeField).IsAssignableFrom(f.GetType()))
                                    {
                                        foreach (var c in (f as CompositeField).Children)
                                        {
                                            List<string> cfFieldValues = c.GetConcatenatedFieldValues(selectedCompositeFieldGuids, " |");
                                            foreach (var val in cfFieldValues) //MR: These are just regular Field
                                            {
                                                //Replacing "&" characters with " and ";
                                                var sanitizedVal = val.Replace("&", " and ");
                                                bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", sanitizedVal)));
                                            }

                                        }
                                    }
                                }
                            }




                            int statusIdx = statusIds.IndexOf(item.StatusId);
                            string status;
                            if (statusIdx < 0)
                            {
                                status = _submissionService.GetStatus(item.StatusId).NormalizedStatus;
                                statusIds.Add(item.StatusId);
                                statusVals.Add(status);
                            }
                            else
                                status = statusVals[statusIdx];

                            bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", status)));
                        }
                    }
                }

                resultString = result.ToString();
            }

            if (!string.IsNullOrEmpty(errorMessage))
                errorMessage = string.Format("<div class='alert alert-danger'>{0}</div>", errorMessage);

            return errorMessage + resultString;
        }

     
        // POST api/<ItemController>
        [Route("SubmitForm")]
        [HttpPost]
        public ApiResult SubmitForm([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] Guid? groupId, [FromForm] string actionButton, [FromForm] Guid stateId, [FromForm] Guid postActionId, [FromForm] string fileNames=null)
        {
            ApiResult result = new ApiResult();
            try
            {
                Item newItem = _submissionService.SetSubmission(value, entityTemplateId, collectionId, groupId, stateId, actionButton);
                _appDb.Items.Add(newItem);
                _appDb.SaveChanges();

                bool triggerStatus = _jobService.ProcessTriggers(newItem.Id);

                bool triggerExecute = _submissionService.ExecuteTriggers(entityTemplateId, newItem, postActionId);

                
                result.Success = true;
                result.Message = _submissionService.SetSuccessMessage(entityTemplateId, postActionId, newItem.Id);
                //if (actionButton == "Save")
                //    result.Message = "Form saved successfully.";
                //else if (actionButton == "Submit")
                //    result.Message = "Form submitted successfully.";
                //else if (actionButton == "Delete")
                //    result.Message = "Form deleted successfully.";
                //else
                //    result.Message = "Task completed successfully.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Submission failed.";
            }

            return result;
        }

        // POST api/<ItemController>
        [Route("EditSubmissionForm")]
        [HttpPost]
        public ApiResult EditSubmission([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] Guid itemId, [FromForm] Guid? groupId, [FromForm] string actionButton, [FromForm] Guid status, [FromForm] Guid postActionId, [FromForm] string fileNames = null)
        {
            ApiResult result = new ApiResult();
            try
            {
                Item newItem = _submissionService.EditSubmission(value, entityTemplateId, collectionId,itemId, groupId, status, actionButton);
                //_appDb.Items.Add(newItem);
                //_appDb.SaveChanges();

                bool triggerStatus = _jobService.ProcessTriggers(newItem.Id);

                bool triggerExecute = _submissionService.ExecuteTriggers(entityTemplateId, newItem, postActionId);

                _appDb.Items.Update(newItem);
                _appDb.SaveChanges();

                result.Success = true;

                result.Message = _submissionService.SetSuccessMessage(entityTemplateId, postActionId, itemId);
                //if (actionButton == "Save")
                //    result.Message = "Form saved successfully.";
                //else if (actionButton == "Submit")
                //    result.Message = "Form submitted successfully.";
                //else if (actionButton == "Delete")
                //    result.Message = "Form deleted successfully.";
                //else
                //    result.Message = "Task completed successfully.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Submission failed.";
            }

            return result;
        }


        [Route("AutoSave")]
        [HttpPost]
        public ApiResult AutoSave([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid itemId)
        {
            ApiResult result = new ApiResult();
            try
            {
                Backup backup = _appDb.Backups.Where(bk => bk.Id == value.Id).FirstOrDefault();
                if(backup == null)
                {
                    backup = new Backup() { Id = value.Id };
                    _appDb.Backups.Add(backup);
                }

                backup.SourceData = value.Content;
                backup.SourceId = itemId;
                backup.SourceType = "DataItem Backup - EntityTemplateId: " + entityTemplateId.ToString();
                backup.Timestamp = DateTime.Now;
                User user = _workflowService.GetLoggedUser();
                if (user != null)
                {
                    backup.UserId = user.Id;
                    backup.Username = user.UserName;
                }

                _appDb.SaveChanges();

                result.Success = true;
                result.Message = "Auto-save successful.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Auto-save failed.";
            }

            return result;
        }


        // POST api/<ItemController>
        [Route("AddChild")]
        [HttpPost]
        public ApiResult AddChild([FromForm] DataItem value, [FromForm] Guid entityTemplateId,  [FromForm] Guid itemId, [FromForm] Guid postActionId,  [FromForm] Guid stateId, [FromForm] Guid buttonId, [FromForm] string fileNames = null)
        {
            ApiResult result = new ApiResult();
            try
            {
                Item newItem = _submissionService.AddChild(value, entityTemplateId, itemId, stateId, buttonId);
                _appDb.Items.Update(newItem);
                _appDb.SaveChanges();

                bool triggerStatus = _jobService.ProcessTriggers(newItem.Id);

                bool triggerExecute = _submissionService.ExecuteTriggers(entityTemplateId, newItem, postActionId);


                result.Success = true;
                result.Message = _submissionService.SetSuccessMessage(entityTemplateId, postActionId, itemId);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Submission failed.";
            }

            return result;
        }


        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        /// <summary>
        /// This method's called from Submission Block to get Function/group name from selected Item Template workflow
        /// </summary>
        /// <param name="id">This is Item Template Id</param>
        /// 
        /// Modified : April 28 2021 -- add option to retrieve sets of metadatasets instead of action/function that on the ItemTemplate
        /// <returns></returns>
        [HttpGet("getSelectListItem/{id}/{metadataset}")]
        public List<SelectListItem> GetSelectListItem(string id, bool metadataset=false)
        {
           
            List<SelectListItem> result = new List<SelectListItem>();
            if (!metadataset)
            {
                if (!string.IsNullOrEmpty(id) && id.ToLower() != "null")
                {
                    var actions = _entityTemplateService.GetTemplateActions(Guid.Parse(id));

                    foreach (Core.Models.Contents.Workflow.GetAction action in actions)
                    {
                        result.Add(new SelectListItem { Text = action.Function, Value = action.Function + "|" + action.Group });

                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(id) && id.ToLower() != "null")
                {
                    var metadataSets = _entityTemplateService.GetTemplateMetadataSets(Guid.Parse(id));

                    foreach (MetadataSet ms in metadataSets)
                    {
                        result.Add(new SelectListItem { Text = ms.Name.GetConcatenatedContent("|"), Value = ms.Id.ToString() });

                    }
                }
            }

            result = result.OrderBy(li => li.Text).ToList();
            return result ;
        }

        [Route("SaveFiles")]
        [HttpPost]
        public IActionResult SaveFiles(ICollection<IFormFile> files)
        {
            //Dictionary<string, List<string>> dictFileNames = new Dictionary<string, List<string>>();
            string dictFileNames = ""; //"key1:file1,file2 | key2:file1,file2"
            foreach (IFormFile f in files)
            {
                string newGuid = Guid.NewGuid().ToString(); //System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                //create temp directory for login user
                if (!Directory.Exists("wwwroot/uploads/temp"))
                    Directory.CreateDirectory("wwwroot/uploads/temp");

                string fileN = newGuid + "_" + f.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", fileN);

                string[] _fileNames = f.FileName.Split("__"); //this will get the field index ==> filed_4, this will be the key for the file name(s) of this field
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    f.CopyTo(stream);
                    //fileNames.Add(fileN);
                    if(!dictFileNames.Contains(_fileNames[1],StringComparison.OrdinalIgnoreCase))
                    {
                        if(string.IsNullOrWhiteSpace(dictFileNames))
                            dictFileNames = _fileNames[1] + ":" + fileN; //still empty
                        else
                            dictFileNames += "|" + _fileNames[1] + ":" + fileN;
                    }
                    else
                    {
                        dictFileNames += "," + fileN;
                    }
                   // dictFileNames.Add(_fileNames[1], new List<string>(listfnames));
                }
            }
            return Ok(dictFileNames);
        }

        [Route("{itemId}/{dataItemId}/{fieldId}/{fileName}")]
        public IActionResult GetFile(Guid itemId, Guid dataItemId, Guid fieldId, string fileName)
        {
            try
            {
                var item = _appDb.Items.Where(it => it.Id == itemId).FirstOrDefault();
                var dataItem = item.DataContainer.Where(di => di.Id == dataItemId).FirstOrDefault();
                var attField = dataItem.Fields.Where(field => field.Id == fieldId).FirstOrDefault() as AttachmentField;
                var fileRef = attField.Files.Where(fr => fr.FileName == fileName).FirstOrDefault();

                var task = _dotnetAuthorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read });
                task.Wait();

                if (task.Result.Succeeded)
                {
                    string pathName = Path.Combine(ConfigHelper.GetAttachmentsFolder(false), fileRef.FileName);
                    if (System.IO.File.Exists(pathName))
                    {
                        var data = System.IO.File.ReadAllBytes(pathName);
                        return File(data, fileRef.ContentType, fileRef.OriginalFileName);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return NotFound();
        }
    }
}
