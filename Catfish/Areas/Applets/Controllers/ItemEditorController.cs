using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Services;
using ElmahCore;
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
        private readonly ISubmissionService _submissionService;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        public ItemEditorController(ISubmissionService submissionService, IEntityTemplateService entityTemplateService, IWorkflowService workflowService, AppDbContext appDb, ErrorLog errorLog)
        {
            _submissionService = submissionService;
            _entityTemplateService = entityTemplateService;
            _workflowService = workflowService;
            _appDb = appDb;
            _errorLog = errorLog;
        }
        [HttpGet]
        [Route("itemeditor/{itemId:Guid}")]
        public Item StateTransitions(Guid itemId)
        {
            Item item = _submissionService.GetSubmissionDetails(itemId);
            EntityTemplate template = _entityTemplateService.GetTemplate(item.TemplateId);
            List<PostAction> postActions = _workflowService.GetAllChangeStatePostActions(template, item.StatusId);
            return null;
        }
    }
}
