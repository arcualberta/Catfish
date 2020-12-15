using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IList<string> GetItemList(Guid templateId, Guid? collectionId, DateTime? startDate, DateTime? endDate)
        {
            //Making sure the startDate is trimmed to the begining of the day and the endDate is bumped up to the end of the day
            if(startDate.HasValue)
                startDate = startDate.Value.Date;
            if (endDate.HasValue)
                endDate = endDate.Value.Date.AddDays(1);

            List<string> itemFields = new List<string>();
            EntityTemplate template = _entityTemplateService.GetTemplate(templateId, User);
            if (template != null)
            {

                IList<Item> itemList = _submissionService.GetSubmissionList(User, templateId, collectionId, startDate, endDate);
               
                bool header = true;
                if (itemList.Count > 0)
                {
                    foreach (Item item in itemList)
                    {
                        //get all the custom fields from the form
                        var iFields = _submissionService.GetAllField(item.Data.ToString());

                        //get the date when the form is submitted
                        iFields.Add(new ItemField { FieldName = "Created", FieldValue = item.Created.ToShortDateString() });
                        //get the form status
                        string status = _submissionService.GetStatus(item.StatusId).NormalizedStatus;
                        iFields.Add(new ItemField { FieldName = "Status", FieldValue = status });

                        if (header)
                        {
                            string strHeader = "";
                            foreach (var field in iFields)
                            {
                                strHeader += field.FieldName + ",";
                            }
                            itemFields.Add(strHeader);
                            header = false; //only het the header once
                        }

                        //the the filed values
                        string strValues = "";
                        foreach (var field in iFields)
                        {
                            strValues += field.FieldValue + ",";
                        }

                        itemFields.Add(strValues);
                    }

                }//if have some items
               
            }

            return itemFields;
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
    }
}
