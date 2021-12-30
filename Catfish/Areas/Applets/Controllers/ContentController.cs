using Catfish.Areas.Applets.Models.Blocks;
using Catfish.Models;
using ElmahCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Piranha.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Controllers
{
    [Route("applets/api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly Piranha.AspNetCore.Identity.IDb _piranhaDb;
        private readonly ErrorLog _errorLog;

        public ContentController(IModelLoader loader, Piranha.AspNetCore.Identity.IDb piranhaDb, ErrorLog errorLog)
		{
            _loader = loader;
            _piranhaDb = piranhaDb;
            _errorLog = errorLog;
        }

        [HttpGet]
        [Route("page/{pageId:Guid}")]
        public async Task<ContentResult> Grid(Guid pageId)
        {
            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return Content(JsonConvert.SerializeObject(page, settings), "application/json");
        }

        [HttpGet]
        [Route("page/{pageId:Guid}/block/{blockId:Guid}")]
        public async Task<ContentResult> Grid(Guid pageId, Guid blockId)
        {
            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
            var block = page?.Blocks.FirstOrDefault(b => b.Id == blockId);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return Content(JsonConvert.SerializeObject(block, settings), "application/json");
        }
    }
}
