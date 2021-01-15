using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ItemsController(AppDbContext db, IEntityTemplateService entityTemplateService, ISubmissionService submissionService)
        {
            _entityTemplateService = entityTemplateService;
            _submissionService = submissionService;
           
           
            _appDb = db;
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
        [HttpPost]
        public ApiResult Post([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] Guid? groupId, [FromForm] string actionButton,  [FromForm] string function,  [FromForm] string group, [FromForm] string status)
        {
            ApiResult result = new ApiResult();
            try
            {
                Item newItem = _submissionService.SetSubmission(value, entityTemplateId, collectionId, groupId, status, actionButton);
                _appDb.Items.Add(newItem);
                _appDb.SaveChanges();

                bool triggerExecute = _submissionService.ExecuteTriggers(entityTemplateId, actionButton, function, group);
                result.Success = true;
                result.Message = "Application "+ actionButton+" successfully.";

            }
            catch(Exception ex)
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
        /// <returns></returns>
        [HttpGet("getSelectListItem/{id}")]
        public List<SelectListItem> GetSelectListItem(string id)
        {
           
            List<SelectListItem> result = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                var actions = _entityTemplateService.GetTemplateActions(Guid.Parse(id));
            
                foreach(Core.Models.Contents.Workflow.GetAction action in actions)
                {
                   result.Add(new SelectListItem { Text = action.Function, Value = action.Function + "|" + action.Group });
                 
                }
            }
            return result ;
        }
    }
}
