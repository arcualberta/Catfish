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
        private readonly AppDbContext _db;

        [BindProperty]
        public DataItem Item { get; set; }

        [BindProperty]
        public Guid TemplateId { get; set; }
             
        public CreateEntity(IWorkflowService workflow, IEntityTemplateService temp, AppDbContext db) : base(null, null)
        {
            _workflowService = workflow;
            _entityTemplateService = temp;
            _db = db;
        }
        public void OnGet(Guid templateId)
        {
            TemplateId = templateId;
            EntityTemplate template = _entityTemplateService.GetTemplate(TemplateId);

            Item = template.GetRootDataItem(false);
        }

        public IActionResult OnPost()
        {
            //Creating a clone of the entity
            EntityTemplate template = _entityTemplateService.GetTemplate(TemplateId);
            if (template == null)
                throw new Exception("Entity template with ID = " + TemplateId + " not found.");

            //When we instantantiate an instance from the template, we do not need to clone metadata sets
            Item newItem = template.Instantiate<Item>();

            DataItem newDataItem = template.InstantiateDataItem(this.Item.Id);
            newDataItem.UpdateFieldValues(this.Item);
            newItem.DataContainer.Add(newDataItem);

            //Adding the new entity to the database
            _db.Items.Add(newItem);
            _db.SaveChanges();

            return RedirectToPage("EntityDetailsPage", new { id = newItem.Id });
        }
    }
}