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
        public ItemsController(IEntityTemplateService entityTemplateService)
        {
            _entityTemplateService = entityTemplateService;
        }
        // GET: api/<ItemController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ItemController>
        [HttpPost]
        public void Post([FromForm] Item value, Guid TemplateId)
        {
            EntityTemplate template = _entityTemplateService.GetTemplate(value.TemplateId);
            if (template == null)
                throw new Exception("Entity template with ID = " + TemplateId + " not found.");

            //When we instantantiate an instance from the template, we do not need to clone metadata sets
            Item newItem = template.Instantiate<Item>();

            //DataItem newDataItem = template.InstantiateDataItem(value.Id);
            //newDataItem.UpdateFieldValues(value);
            //newItem.DataContainer.Add(newDataItem);
            //newDataItem.EntityId = newItem.Id;

            //TODO: associated the newly createditem with the collection specified by CollectionId.

            //Adding the new entity to the database
            //_db.Items.Add(newItem);
            //_db.SaveChanges();
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
