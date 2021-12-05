using Catfish.Areas.Applets.Services;
using Catfish.Core.Models;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}
