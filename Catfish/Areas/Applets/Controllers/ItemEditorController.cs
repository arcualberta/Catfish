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
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ItemEditorController(IItemAppletService itemAppletService, ISubmissionService submissionService, IEntityTemplateService entityTemplateService, IWorkflowService workflowService, AppDbContext appDb, ErrorLog errorLog)
        {
            _itemAppletService = itemAppletService;

            _submissionService = submissionService;
            _entityTemplateService = entityTemplateService;
            _workflowService = workflowService;
            _appDb = appDb;
            _errorLog = errorLog;
        }
        [HttpGet]
        [Route("itemeditor/{id:Guid}")]
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
        public Item Get(Guid id)
        {
            Item item = _itemAppletService.GetItem(id, User);

            return item;
        }

    }
}
