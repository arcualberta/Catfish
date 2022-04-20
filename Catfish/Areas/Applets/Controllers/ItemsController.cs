using Catfish.Areas.Applets.Authorization;
using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using Catfish.Areas.Applets.Services;
using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Exceptions;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Reports;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catfish.Areas.Applets.Controllers
{
    [Route("applets/api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemAppletService _itemAppletService;
        private readonly ISubmissionService _submissionService;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IWorkflowService _workflowService;
        private readonly IJobService _jobService;
        private readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        private readonly IItemAuthorizationHelper _itemAuthorizationHelper;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _authorizationService;
        public ItemsController(IItemAppletService itemAppletService, ISubmissionService submissionService, IEntityTemplateService entityTemplateService, IWorkflowService workflowService, IJobService jobService, AppDbContext appDb, IItemAuthorizationHelper itemAuthorizationHelper, ErrorLog errorLog, Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService)

        {
            _itemAppletService = itemAppletService;
            _itemAuthorizationHelper = itemAuthorizationHelper;
            _submissionService = submissionService;
            _entityTemplateService = entityTemplateService;
            _workflowService = workflowService;
            _jobService = jobService;
            _authorizationService = authorizationService;
            _appDb = appDb;
            _errorLog = errorLog;
        }

        [HttpGet]
        [Route("statetranistions/{id:Guid}")]
        public List<string> StateTransitions(Guid id)
        {
            Item item = _submissionService.GetSubmissionDetails(id);
            EntityTemplate template = _entityTemplateService.GetTemplate(item.TemplateId);
            List<PostAction> postActions = _workflowService.GetAllChangeStatePostActions(template, item.StatusId);
            List<string> nextButtons = new List<string>();
            foreach(var postAction in postActions)
            {
                foreach(var stateMapping in postAction.StateMappings)
                {
                    if (stateMapping.Current == item.StatusId)
                        nextButtons.Add(stateMapping.ButtonLabel);
                }
            }

            return nextButtons;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ContentResult> GetAsync(Guid id)
        {
            //We should ultimately implement permission checking using resource-based authorization.
            //For the time being, we are going to implement a role-based authorization that is further limits access based on
            //groups as follows:
            //
            //We grant the user access to an item if
            //  1. Get the group where the instance belongs to
            //  2. Check the roles which grants the user Read privilege on the template
            //  3. See if the current user holds at least one of these roles within the group associated with the item.
            //     If the user holds such a role, then grant access to the item; otherwise, deny permission.
            //  4. Note: if the instance is not associated with any group, then simply check for all roles which grant the user
            //     Read privilege and see if the current user has one of such roles 


            Item item = _appDb.Items.FirstOrDefault(it => it.Id == id);
            item.Template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == item.TemplateId);
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read })).Succeeded)
            {
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return Content(JsonConvert.SerializeObject(item, settings), "application/json");
            }
            else
                return Content("{}", "application/json");

        }

        [HttpPost]
        //[Route("{templateId:Guid}")]
        public async Task<ContentResult> PostAsync(Guid itemTemplateId, Guid? groupId, Guid collectionId, [FromForm] String datamodel, [FromForm] List<IFormFile> files, [FromForm] List<string> fileKeys)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            DataItem itemInstance = JsonConvert.DeserializeObject<DataItem>(datamodel, settings);

            try
            {
                //Guid collectionId = Guid.Parse("9ed65277-6a9e-4a96-c86a-e6825889234a"); ;
                //Guid groupId = Guid.Parse("2BD48E47-3DD7-4DA0-9F07-BEB72EE3542D");

                Guid stateMappingId = _workflowService.GetSubmitStateMappingId(itemTemplateId);//Guid.Parse("57a5509e-6aa2-463c-9cb6-b18178450aca");
                string actionButton = "Submit";
                itemInstance.TemplateId = itemInstance.Id;
                itemInstance.Id = Guid.NewGuid();
                Item newItem = _submissionService.SetSubmission(itemInstance, itemTemplateId, collectionId, groupId, stateMappingId, actionButton, files, fileKeys);


                if ((await _authorizationService.AuthorizeAsync(User, newItem, new List<IAuthorizationRequirement>() { TemplateOperations.Instantiate })).Succeeded)
                {
                    _appDb.Items.Add(newItem);
                    _appDb.SaveChanges();

                    bool triggerStatus = _jobService.ProcessTriggers(newItem.Id);
                }
            }
            catch (Exception ex)
            {

                return Content("{}", "application/json");
            }
            return Content(JsonConvert.SerializeObject("Sucess", settings), "application/json");
        }

        [HttpGet("getChildForm/{instanceId}/{childFormId}")]
        public async Task<ContentResult> GetChildFormAsync(Guid instanceId, Guid childFormId)
        {
            Item item = _appDb.Items.FirstOrDefault(it => it.Id == instanceId);
            item.Template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == item.TemplateId);

            //TODO: Update the following Authorization-checking in order to propoerly check authorizations specified for
            //submitting child forms. For the time being, we limit it to the users who have permission to Read the
            //main submission. This is only a quick shortcut we created to help TBLT site.
            //bool authorizedToSubmitChildForm = _itemAuthorizationHelper.AuthorizebyRole(item, User, "Read");
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
.Succeeded)
            {
                DataItem childForm = item.Template.DataContainer.FirstOrDefault(cf => cf.Id == childFormId);

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return Content(JsonConvert.SerializeObject(childForm, settings), "application/json");
            }
            else
                return Content("{}", "application/json");
            
        }

        [HttpPost]
        [Route("appendchildforminstance/{itemInstanceId}")]
        public async Task<ContentResult> AppendChildFormInstanceAsync(Guid itemInstanceId, [FromForm] Guid? parentId, [FromForm] String datamodel)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            DataItem childForm = JsonConvert.DeserializeObject<DataItem>(datamodel, settings);

            childForm.TemplateId = childForm.Id; //Comment Form Id
            childForm.Id = Guid.NewGuid();
            childForm.Created = DateTime.Now;
            childForm.Updated = childForm.Created;

            
            var item = _appDb.Items.FirstOrDefault(i => i.Id == itemInstanceId);
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
            .Succeeded)
            {
                DataItem parent = parentId.HasValue ? item.DataContainer.FirstOrDefault(di => di.Id == parentId.Value) : null;

                //If a data item with the given parentDataItemId is found in the DataContainer of
                //this item, then add the childForm as a child to that data item. Otherwise, add
                //the child form directly to the data container.
                if (parent != null)
				{
                    childForm.ParentId = parent.Id;
                    parent.ChildFieldContainers.Add(childForm);
                }
                else
                    item.DataContainer.Add(childForm);

                _appDb.SaveChanges();
            }

            return Content(JsonConvert.SerializeObject(childForm, settings), "application/json");
        }

        [HttpGet("getchildformsubmissions/{instanceId}/{childFormId}")]
        public async Task<ContentResult> GetChildFormSubmissionsAsync(Guid instanceId, Guid childFormId, Guid? parentId)
        {
            Item item = _appDb.Items.FirstOrDefault(it => it.Id == instanceId);
            item.Template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == item.TemplateId);

            //TODO: Update the following Authorization-checking in order to propoerly check authorizations specified for
            //submitting child forms. For the time being, we limit it to the users who have permission to Read the
            //main submission. This is only a quick shortcut we created to help TBLT site.
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
            .Succeeded)
            {
                var query = item.DataContainer.Where(c => c.TemplateId == childFormId);
                if (parentId.HasValue)
                    query = query.Where(c => c.ParentId == parentId);

                var childSubmissions = query.OrderByDescending(c => c.Created).ToList();

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return Content(JsonConvert.SerializeObject(childSubmissions, settings), "application/json");
            }
            else
                return Content("{}", "application/json");
        }
        
        [HttpPost("deleteChildForm/{instanceId}/{childFormId}")]
        public async Task<ContentResult> DeleteChildFormAsync(Guid instanceId, Guid childFormId, Guid? parentId)
        {
            Item item = _appDb.Items.FirstOrDefault(it => it.Id == instanceId);
            item.Template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == item.TemplateId);
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.ChildFormDelete }))
            .Succeeded)
            {
                item = _submissionService.DeleteChild(instanceId, childFormId, parentId);
                
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                _appDb.SaveChanges();
                return Content(JsonConvert.SerializeObject(item, settings), "application/json");
            }
            else
            {
                return Content("{}", "application/json");
            }
                
        }
        [HttpPost("deleteItem/{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(Guid itemId)
        {
            //retrive item data according to the item id
            Item item = _appDb.Items.FirstOrDefault(it => it.Id == itemId);
            //check item, if it is not null, then it can process. Otherwise need to return Status404NotFound
            if (item != null)
            {
                //check the user has permission to delete item, if yes, it can process, otherwise return Status401Unauthorized
                if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Delete }))
            .Succeeded)
                {
                    Item deletedItem = _submissionService.DeleteSubmission(item);
                    //check item deleted sucessfully. if yes, return Status200OK, Otherwise return Status500InternalServerError
                    if (deletedItem != null)
                    {
                        _appDb.SaveChanges();
                        return StatusCode(StatusCodes.Status200OK);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost("GetReportData/{groupId}/template/{templateId}/collection/{collectionID}")]
        public ContentResult GetReportData(Guid groupId, Guid templateId, Guid collectionID, [FromForm] String datamodel, DateTime? startDate, DateTime? endDate, Guid? status)
        {
            try
            {
                var deserializationSettings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var fields = JsonConvert.DeserializeObject<ReportDataFields[]>(datamodel, deserializationSettings);

                List<ReportRow> rows = _submissionService.GetSubmissionList(groupId, templateId, collectionID, fields, startDate, endDate, status);

                var serializationSettings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                return Content(JsonConvert.SerializeObject(rows, serializationSettings), "application/json");
            }
            catch (Exception ex)
            {

                return Content("{}", "application/json");
            }

        }

        /// <summary>
        /// Get Item instances for the given item/form ids
        /// </summary>
        /// <param name="itemIds">form instance ids</param>
        /// <returns></returns>
        //[HttpGet("getItems/{templateId}/{itemIds}")]
        [HttpGet("getItems/{templateId}")]
        public List<Item> GetItemsAsync(Guid templateId/*,string itemIds*/)
        {
            //string[] itmIds = itemIds.Split(",");
            //List<DataItem> result = new List<DataItem>();
           
            //EntityTemplate template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == templateId);
            List<Item> items = _appDb.Items.Where(it => it.TemplateId == templateId).ToList();
            //foreach (Item item in items)
            //{
            //    foreach (string itemId in itmIds)
            //    {
            //        //retrive item data according to the item id

            //        //check item, if it is not null, then it can process. Otherwise need to return Status404NotFound
            //        if (item != null)
            //        {
            //            var query = item.DataContainer.Where(c => c.TemplateId == Guid.Parse(itemId));
            //            //if (parentId.HasValue)
            //            //    query = query.Where(c => c.ParentId == parentId);

            //            var dataItem = query.FirstOrDefault();



            //            result.Add(dataItem);

            //        }
            //    }
            //}
            //var settings = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    TypeNameHandling = TypeNameHandling.All,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};
            // return Content(JsonConvert.SerializeObject(items, settings), "application/json");
            return items;//result;
        }
        [HttpGet("getUserPermissions/{itemId}")]
        public List<string> GetUserPermissions(Guid itemId)
        {
            try
            {
                List<string> userPermissions = _itemAppletService.GetUserPermissions(itemId, User);

                return null;
            }
            catch (Exception)
            {

                return null;
            }

            
        }
    }
}
