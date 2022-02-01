using Catfish.Areas.Applets.Authorization;
using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using Catfish.Areas.Applets.Services;
using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Exceptions;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public class ItemEditorController : ControllerBase
    {
        private readonly IItemAppletService _itemAppletService;
        private readonly ISubmissionService _submissionService;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        private readonly IItemAuthorizationHelper _itemAuthorizationHelper;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _authorizationService;
        public ItemEditorController(IItemAppletService itemAppletService, ISubmissionService submissionService, IEntityTemplateService entityTemplateService, IWorkflowService workflowService, AppDbContext appDb, IItemAuthorizationHelper itemAuthorizationHelper, ErrorLog errorLog, Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService)
        {
            _itemAppletService = itemAppletService;
            _itemAuthorizationHelper = itemAuthorizationHelper;
            _submissionService = submissionService;
            _entityTemplateService = entityTemplateService;
            _workflowService = workflowService;
            _appDb = appDb;
            _errorLog = errorLog;
            _authorizationService = authorizationService;
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
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
.Succeeded)
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
        public ContentResult Post([FromForm] String datamodel)
		{
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            DataItem itemInstance = JsonConvert.DeserializeObject<DataItem>(datamodel, settings);
            itemInstance.TemplateId = itemInstance.Id;
            itemInstance.Id = Guid.NewGuid();



            throw new NotImplementedException();
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
        public async Task<DataItem> AppendChildFormInstanceAsync(Guid itemInstanceId, [FromForm] String datamodel)
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
            var item = _appDb.Items.FirstOrDefault(i => i.Id == itemInstanceId);
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
            .Succeeded)
            {
                item.DataContainer.Add(childForm);

                _appDb.SaveChanges();
            }
                

            return childForm;
        }

        [HttpGet("getchildformsubmissions/{instanceId}/{childFormId}")]
        public async Task<ContentResult> GetChildFormSubmissionsAsync(Guid instanceId, Guid childFormId)
        {
            Item item = _appDb.Items.FirstOrDefault(it => it.Id == instanceId);
            item.Template = _appDb.EntityTemplates.FirstOrDefault(t => t.Id == item.TemplateId);

            //TODO: Update the following Authorization-checking in order to propoerly check authorizations specified for
            //submitting child forms. For the time being, we limit it to the users who have permission to Read the
            //main submission. This is only a quick shortcut we created to help TBLT site.
            if ((await _authorizationService.AuthorizeAsync(User, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read }))
            .Succeeded)
            {
                var childSubmissions = item.DataContainer.Where(c => c.TemplateId == childFormId).ToList();

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
    }
}
