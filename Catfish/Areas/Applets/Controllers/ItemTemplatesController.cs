using Catfish.Areas.Applets.Services;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Get all the child form that attached to the item template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet("getItemtemplateChildForms/{templateId}")]
        public List<SelectListItem> GetItemTemplateChildForms(string templateId)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            List<DataItem> dataItems = _itemTemplateAppletService.GetDataItems(Guid.Parse(templateId));

            foreach (DataItem itm in dataItems)
            {
                result.Add(new SelectListItem {Text= itm.GetName("en"), Value=itm.Id.ToString() });

            }
            return result;
        }

		[HttpGet("getChildForm/{templateId}/{childFormId}")]
		public ActionResult GetChildForm(Guid templateId, Guid childFormId)
		{
			//TODO: How do we want to handle security in this case?
			ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == templateId);
			DataItem childForm = template.DataContainer.FirstOrDefault(cf => cf.Id == childFormId);

			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			string jsonString = JsonConvert.SerializeObject(childForm, settings);
			return Content(jsonString, "application/json");
		}

	}
}
