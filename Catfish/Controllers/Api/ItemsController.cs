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
        public IList<string> GetItemList(Guid templateId, Guid collectionId, DateTime startDate, DateTime endDate)
        {
            IList<Item> itemList = _submissionService.GetSubmissionList(templateId,collectionId, startDate, endDate);
            List<string> itemFields = new List<string>();
            bool header = true;
            if (itemList.Count > 0)
            {
                foreach (Item item in itemList) {
                    //get all the custom fields from the form
                    var iFields = _submissionService.GetAllField(item.Data.ToString());

                    //get the date when the form is submitted
                    iFields.Add(new ItemField { FieldName = "Created", FieldValue = item.Created.ToShortDateString() });
                    //get the form status
                    string status = _submissionService.GetStatus(item.StatusId);
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
            

            return itemFields;
        }

        // POST api/<ItemController>
        [HttpPost]
        public ApiResult Post([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] string actionButton)
        {
            ApiResult result = new ApiResult();
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
                if (template == null)
                    throw new Exception("Entity template with ID = " + entityTemplateId + " not found.");

                //When we instantantiate an instance from the template, we do not need to clone metadata sets
                Item newItem = template.Instantiate<Item>();
                newItem.StatusId = _entityTemplateService.GetStatus(entityTemplateId, actionButton, true).Id;
                newItem.PrimaryCollectionId = collectionId;
                newItem.TemplateId = entityTemplateId;

                DataItem newDataItem = template.InstantiateDataItem((Guid)value.TemplateId);
                newDataItem.UpdateFieldValues(value);
                newItem.DataContainer.Add(newDataItem);
                newDataItem.EntityId = newItem.Id;

                //TODO: associated the newly createditem with the collection specified by CollectionId.

                //Adding the new entity to the database
                _appDb.Items.Add(newItem);
                _appDb.SaveChanges();

                result.Success = true;
                result.Message = "Submission saved successfully.";

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
