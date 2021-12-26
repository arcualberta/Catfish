using Catfish.Areas.Applets.Authorization;
using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using Catfish.Areas.Applets.Services;
using Catfish.Core.Exceptions;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using ElmahCore;
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
        public ItemEditorController(IItemAppletService itemAppletService, ISubmissionService submissionService, IEntityTemplateService entityTemplateService, IWorkflowService workflowService, AppDbContext appDb, IItemAuthorizationHelper itemAuthorizationHelper, ErrorLog errorLog)
        {
            _itemAppletService = itemAppletService;
            _itemAuthorizationHelper = itemAuthorizationHelper;
            _submissionService = submissionService;
            _entityTemplateService = entityTemplateService;
            _workflowService = workflowService;
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
        public ContentResult Get(Guid id)
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
            if (_itemAuthorizationHelper.AuthorizebyRole(item, User, "Read"))
            {
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return Content(JsonConvert.SerializeObject(item, settings), "application/json");
            }
            else
                return Content("{}", "application/json");

        }

    }
}
