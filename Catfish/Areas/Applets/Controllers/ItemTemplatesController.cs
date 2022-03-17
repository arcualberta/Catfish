using Catfish.Areas.Applets.Services;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Services;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        private readonly IEntityTemplateService _entityTemplateService;
        public ItemTemplatesController(IItemTemplateAppletService itemTemplateAppletService, AppDbContext appDb,IEntityTemplateService entityTemplateService, ErrorLog errorLog)
        {
            _itemTemplateAppletService = itemTemplateAppletService;
            _appDb = appDb;
            _errorLog = errorLog;
            _entityTemplateService = entityTemplateService;
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
        /// Get all the child forms that attached to the item template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet("getItemtemplateChildForms/{templateId}")]
        public List<SelectListItem> GetItemTemplateChildForms(string templateId)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            List<DataItem> dataItems = _itemTemplateAppletService.GetDataItems(Guid.Parse(templateId), false);

            foreach (DataItem itm in dataItems)
            {
                result.Add(new SelectListItem {Text= itm.GetName("en"), Value=itm.Id.ToString() });

            }
            return result;
        }

        /// <summary>
        /// Get all the root forms that attached to the item template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet("getItemTemplateRootForms/{templateId}")]
        public List<SelectListItem> GetItemTemplateRootForms(string templateId)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            List<DataItem> dataItems = _itemTemplateAppletService.GetDataItems(Guid.Parse(templateId), true);

            foreach (DataItem itm in dataItems)
            {
                result.Add(new SelectListItem { Text = itm.GetName("en"), Value = itm.Id.ToString() });

            }
            return result;
        }

        [HttpGet("getAllItemTemplateForms/{templateId}")]
        public List<SelectListItem> GetAllItemTemplateForms(string templateId)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            List<DataItem> dataItems = _itemTemplateAppletService.GetAllDataItems(Guid.Parse(templateId));

            foreach (DataItem itm in dataItems)
            {
                result.Add(new SelectListItem { Text = itm.GetName("en"), Value = itm.Id.ToString() });

            }
            return result;
        }

        [HttpGet("getAllCollectionsRelatedToGroupTemplate/{templateId}/{groupId}")]
        public List<SelectListItem> GetAllCollectionsRelatedToGroupTemplate(Guid templateId, Guid groupId)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            ICollection<Collection> collections = _itemTemplateAppletService.GetAllGroupTemplateCollections(templateId, groupId);
            foreach (Collection collection in collections)
            {
                result.Add(new SelectListItem { Text = collection.ConcatenatedName, Value = collection.Id.ToString() });

            }

            return result;
        }

            [HttpGet("{templateId}/data-form/{formId}")]
        public ContentResult DataForm(Guid templateId, Guid formId)
        {
            //TODO: Implement security

            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == templateId);
            DataItem dataForm = template.DataContainer.FirstOrDefault(di => di.Id == formId);
            bool authorizationSuccessful = true;

			if (authorizationSuccessful)
			{

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return Content(JsonConvert.SerializeObject(dataForm, settings), "application/json");
            }
            else
                return Content("{}", "application/json");
        }

        [HttpGet("getItemtemplateFields/{templateId}/{dataItemId}")]
        public List<ReportField> GetItemTemplateFields(string templateId, string dataItemId)
        {

            // List<SelectListItem> result = new List<SelectListItem>();
            List<ReportField> result = new List<ReportField>();
            if (!string.IsNullOrEmpty(templateId))
            {
                ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == Guid.Parse(templateId));
                DataItem dataForm = template.DataContainer.FirstOrDefault(di => di.Id == Guid.Parse(dataItemId));

                var fields = _entityTemplateService.GetTemplateDataItemFields(Guid.Parse(templateId), Guid.Parse(dataItemId));
                
                SelectListGroup group = new SelectListGroup();
                group.Name = dataItemId + ":" + dataForm.GetName("en");
                


                foreach (BaseField field in fields)
                {
                    if (!string.IsNullOrEmpty(field.GetName()))
                    {
                        ReportField rf = new ReportField();
                        rf.FormId = dataItemId;
                        rf.FormName = dataForm.GetName("en");
                        rf.FieldId = field.Id.ToString();
                        rf.FieldName = field.GetName();
                        // result.Add(new SelectListItem { Text = field.GetName(), Value = field.Id.ToString(), Group=group });
                        result.Add(rf);
                    }

                }
            }

            result = result.OrderBy(li => li.FieldName).ToList();
            return result;
        }
        /// <summary>
        /// Get mutiple form fields
        /// </summary>
        /// <param name="templateId">Item template id</param>
        /// <param name="dataItemIds">Ids of the forms</param>
        /// <returns></returns>
        [HttpGet("getMutipleFormFields/{templateId}/{dataItemIds}")]
        public Dictionary<string, List<ReportField>> GetMutipleFormFields(string templateId, string dataItemIds)
        {

            // List<SelectListItem> result = new List<SelectListItem>();
            Dictionary<string, List<ReportField>> result = new Dictionary<string, List<ReportField>>();
            string[] formIds = dataItemIds.Split(",");

            if (!string.IsNullOrEmpty(templateId))
            {
               
                foreach (string dataItemId in formIds)
                {
                    if (!string.IsNullOrEmpty(dataItemId))
                    {
                        List<ReportField> rptFields = new List<ReportField>();
                        ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == Guid.Parse(templateId));
                        DataItem dataForm = template.DataContainer.FirstOrDefault(di => di.Id == Guid.Parse(dataItemId));

                        var fields = _entityTemplateService.GetTemplateDataItemFields(Guid.Parse(templateId), Guid.Parse(dataItemId));

                        SelectListGroup group = new SelectListGroup();
                        group.Name = dataItemId + ":" + dataForm.GetName("en");



                        foreach (BaseField field in fields)
                        {
                            if (!string.IsNullOrEmpty(field.GetName()))
                            {
                                ReportField rf = new ReportField();
                                rf.FormId = dataItemId;
                                rf.FormName = dataForm.GetName("en");
                                rf.FieldId = field.Id.ToString();
                                rf.FieldName = field.GetName();
                                // result.Add(new SelectListItem { Text = field.GetName(), Value = field.Id.ToString(), Group=group });
                                rptFields.Add(rf);
                            }

                        }

                        rptFields = rptFields.OrderBy(li => li.FieldName).ToList();
                        result.Add(dataItemId, rptFields);
                    }
                }

               // result = result.OrderBy(li => li.FieldName).ToList();
            }
            return result;
        }

    }

    public class ReportField
    {
        public string FormId { get; set; }
        public string FormName { get; set; }
        public string FieldId { get; set; }
        public string FieldName { get; set; }
    }
}
