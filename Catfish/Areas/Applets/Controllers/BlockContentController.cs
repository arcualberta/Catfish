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
    public class BlockContentController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly Piranha.AspNetCore.Identity.IDb _piranhaDb;
        private readonly ErrorLog _errorLog;

        public BlockContentController(IModelLoader loader, Piranha.AspNetCore.Identity.IDb piranhaDb, ErrorLog errorLog)
		{
            _loader = loader;
            _piranhaDb = piranhaDb;
            _errorLog = errorLog;
        }

        [HttpGet]
        [Route("page/{pageId:Guid}/grid/{gridId:Guid}")]
        public async Task<ContentResult> Grid(Guid pageId, Guid gridId)
        {
            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
            var block = page?.Blocks.FirstOrDefault(b => b.Id == gridId) as Grid;

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return Content(JsonConvert.SerializeObject(block, settings), "application/json");
        }
    }
}
