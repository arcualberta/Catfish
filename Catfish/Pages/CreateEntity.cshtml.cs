using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.ModelBinders;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class CreateEntity : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;
        private readonly IEntityTemplateService _entityTemplateService;

        [BindProperty]
        public DataItem Item { get; set; }
        [BindProperty]
        public Guid TemplateId { get; set; }
             
        public CreateEntity(IWorkflowService workflow, IEntityTemplateService temp) : base(null, null)
        {
            _workflowService = workflow;
            _entityTemplateService = temp;
        }
        public void OnGet(Guid templateId)
        {
            TemplateId = templateId;
            EntityTemplate template = _entityTemplateService.GetTemplate(TemplateId);

            _workflowService.SetModel(template);

            //TODO: dynamically figure out the start-up item
            Item = _workflowService.GetDataItem("Contract Letter", false);
        }

        public void OnPost()
        {
            //Creating a clone of the entity
            EntityTemplate template = _entityTemplateService.GetTemplate(TemplateId);
            Entity entity = template.Clone<Entity>();

            //updating the data item of the entity

            ////var val_1 = Request.Form["DataItem.Fields[0].Values[0].Values[0].Value"];
            ////var val_2 = Request.Form["DataItem.Fields[1].Values[0].Values[0].Value"];
        }
    }
}