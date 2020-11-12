using System;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Piranha.Manager.Models;
using AsyncResult = Piranha.Manager.Models.AsyncResult;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ItemService _srv;
        private ISolrIndexService<SolrEntry> solrIndexService;
        public ItemsController(ItemService srv)
        {
            _srv = srv;
        }
        // GET: api/Items
        [HttpGet]
        public ActionResult Get(int offset = 0, int max = 0)
        {
            ItemListVM vm = _srv.GetItems(offset, max);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(vm, settings);
            return Content(jsonString, "application/json");
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var item = _srv.GetItem(id);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(item, settings);
            return Content(jsonString, "application/json");
            //return Json(vm,, JsonRequestBehavior.AllowGet);
        }

        // POST: api/Items
        [HttpPost]
        public AsyncResult Save(Item model)
        {
            try
            {
                _srv.UpdateItemlDataModel(model);
                
                //TODO:SOLR update
                //solrIndexService.AddUpdate(new SolrItemModel(model));
            }
            catch
            {
                return new AsyncResult
                {
                    Status = new StatusMessage
                    {
                        Type = StatusMessage.Error,
                        Body = "An error occurred while saving"
                    }
                };
            }
            return new AsyncResult
            {
                Status = new StatusMessage
                {
                    Type = StatusMessage.Success,
                    Body = "The Item was successfully saved"
                }
            };
        }

        //public async Task<IActionResult> EditSave(Item model)
        //{

        //    //await _srv.UpdateItemlDataModel(model);

        //    return Ok(model);
        //}

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("index")]
        [HttpPost]
        public static void IndexSite([FromForm] Guid siteId)
        {
            var site = siteId;
        }
    }
}
