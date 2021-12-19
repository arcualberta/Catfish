using Catfish.Areas.Applets.Services;
using Catfish.Core.Models;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Catfish.Areas.Applets.Controllers
{
    [Route("applets/api/[controller]")]
    [ApiController]
    public class ItemTemplatesController : ControllerBase
    {
        private readonly IItemTemplateAppletService _itemTemplateAppletService;
        private readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        public ItemTemplatesController(IItemTemplateAppletService itemTemplateAppletService, AppDbContext appDb, ErrorLog errorLog)
        {
            _itemTemplateAppletService = itemTemplateAppletService;
            _appDb = appDb;
            _errorLog = errorLog;
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public ItemTemplate Get(Guid id)
        {
            ItemTemplate template = _itemTemplateAppletService.GetItemTemplate(id, User);

            return template;
        }

        [HttpGet]
        [Route("groups/{id:Guid}")]
        public List<SelectListItem> GetAssociatedGroups(string id)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            if (! string.IsNullOrEmpty(id))
            {
                List<Group> groups = _itemTemplateAppletService.GetTemplateGroups(Guid.Parse(id));
               
                foreach (var g in groups)
                {
                    result.Add(new SelectListItem { Text = g.Name, Value = g.Id.ToString() });
                }

            }
            return result;

        }
    }
}
