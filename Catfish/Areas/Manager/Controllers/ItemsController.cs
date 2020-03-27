using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ItemService _srv;
        public ItemsController(ItemService srv)
        {
            _srv = srv;
        }
        // GET: api/Items
        [HttpGet]
        public ItemListVM Get(int offset = 0, int max = 0)
        {
            ItemListVM vm = _srv.GetItems(offset, max);
            return vm;
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Items
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

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
    }
}
