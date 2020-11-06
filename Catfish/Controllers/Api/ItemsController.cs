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
        [HttpGet("{id}")]
        public IList<Item> GetItemList(Guid templateId, Guid collectionId)
        {
            IList<Item> itemList = _submissionService.GetSubmissionList(templateId, collectionId);
            return itemList;
        }

        // POST api/<ItemController>
        [HttpPost]
        public void Post([FromForm] DataItem value, [FromForm] Guid entityTemplateId, [FromForm] Guid collectionId, [FromForm] string actionButton)
        {
            EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
            if (template == null)
                throw new Exception("Entity template with ID = " + entityTemplateId + " not found.");

            //When we instantantiate an instance from the template, we do not need to clone metadata sets
            Item newItem = template.Instantiate<Item>();
            newItem.StatusId = _entityTemplateService.GetStatusId(entityTemplateId, actionButton);
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
