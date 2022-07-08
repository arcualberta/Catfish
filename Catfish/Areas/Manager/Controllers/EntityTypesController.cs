using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catfish.Core.Services;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Newtonsoft.Json;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class EntityTypesController : ControllerBase
    {
        protected EntityTypeService Srv { get; set; }

        public EntityTypesController(EntityTypeService srv)
            :base()
        {
            Srv = srv;
        }

        // GET: api/EntityTypes
        [HttpGet]
        public ActionResult Get()
        {
            List<EntityTemplateListEntry> templates = Srv.GetEntityTemplateListEntries().ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(templates, settings);
            return Content(jsonString, "application/json");
        }

        // GET: api/EntityTypes/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EntityTypes
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/EntityTypes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
