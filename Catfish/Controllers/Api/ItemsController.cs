using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly ISubmissionService _submissionService;
       
        private readonly AppDbContext _appDb;
        private readonly IJobService _jobService;
        public ItemsController(AppDbContext db, IEntityTemplateService entityTemplateService, ISubmissionService submissionService, IJobService jobService, IConfiguration configuration)
        {
            _entityTemplateService = entityTemplateService;
            _submissionService = submissionService;

            _appDb = db;
            _jobService = jobService;

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
        public string GetItemList(Guid templateId, Guid? collectionId, DateTime? startDate, DateTime? endDate)
        {
            //Making sure the startDate is trimmed to the begining of the day and the endDate is bumped up to the end of the day
            if (startDate.HasValue)
                startDate = startDate.Value.Date;
            if (endDate.HasValue)
                endDate = endDate.Value.Date.AddDays(1);

            EntityTemplate template = _entityTemplateService.GetTemplate(templateId, User);
            if (template != null)
            {
                XElement result = new XElement("table");
                result.SetAttributeValue("class", "table");

                XElement thead = new XElement("thead");
                result.Add(thead);

                XElement headRow = new XElement("tr");
                thead.Add(headRow);

                DataItem root = template.GetRootDataItem(false);
                if (root != null)
                {
                    var fieldList = root.GetValueFields();

                    headRow.Add(XElement.Parse("<th></th>"));

                    headRow.Add(XElement.Parse("<th>Submission Date</th>"));

                    foreach (var field in fieldList)
                        headRow.Add(XElement.Parse(string.Format("<th>{0}</th>", field.Name.GetConcatenatedContent(" | "))));
                    
                    headRow.Add(XElement.Parse("<th>Status</th>"));

                    XElement tbody = new XElement("tbody");
                    result.Add(tbody);

                    var fieldGuids = fieldList.Select(field => field.Id).ToList();

                    List<Item> itemList = _submissionService.GetSubmissionList(User, templateId, collectionId, startDate, endDate);
                    
                    //Arrays to store already loaded status values instead of having to load them repeatedly from the database
                    List<Guid?> statusIds = new List<Guid?>();
                    List<string> statusVals = new List<string>();

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
                        List<string> fieldValues = dataItem.GetConcatenatedFieldValues(fieldGuids, " |");
                        foreach (var val in fieldValues)
                            bodyRow.Add(XElement.Parse(string.Format("<td >{0}</td>", val)));

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

                return result.ToString();
            }
            return "";
        }

     
        // POST api/<ItemController>
        [Route("SubmitForm")]
        [HttpPost]
        public ApiResult SubmitForm([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] Guid? groupId, [FromForm] string actionButton, [FromForm] string function, [FromForm] string group, [FromForm] string status, [FromForm] string fileNames=null)
        {
            ApiResult result = new ApiResult();
            try
            {
                Item newItem = _submissionService.SetSubmission(value, entityTemplateId, collectionId, groupId, status, actionButton);
                _appDb.Items.Add(newItem);
                _appDb.SaveChanges();

                bool triggerStatus = _jobService.ProcessTriggers(newItem.Id);

                bool triggerExecute = _submissionService.ExecuteTriggers(entityTemplateId, actionButton, function, group);
                result.Success = true;
                result.Message = "Application " + actionButton + " successfully.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Submission failed.";
            }

            return result;
        }

        [Route("DetailsUpdate")]
        [HttpPost]
        public ApiResult DetailsUpdate([FromForm] Guid entityId, [FromForm] Guid currentStatus, [FromForm] Guid status, [FromForm] string buttonName)
        {
            ApiResult result = new ApiResult();
            Item item = _submissionService.StatusChange(entityId, currentStatus, status, buttonName);
            _appDb.Items.Update(item);
            _appDb.SaveChanges();
            result.Success = true;
            result.Message = "Application " + buttonName + " successfully.";
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
        /// <returns></returns>
        [HttpGet("getSelectListItem/{id}")]
        public List<SelectListItem> GetSelectListItem(string id)
        {
           
            List<SelectListItem> result = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id) && id.ToLower() != "null")
            {
                var actions = _entityTemplateService.GetTemplateActions(Guid.Parse(id));
            
                foreach(Core.Models.Contents.Workflow.GetAction action in actions)
                {
                   result.Add(new SelectListItem { Text = action.Function, Value = action.Function + "|" + action.Group });
                 
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
    }
}
