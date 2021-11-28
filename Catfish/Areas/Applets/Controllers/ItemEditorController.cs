﻿using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
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
    }
}
