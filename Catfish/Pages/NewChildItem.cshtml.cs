using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.Data;

namespace Catfish.Pages
{
    public class NewChildItemModel : CatfishPageModelModel
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _db;
        
        [BindProperty]
        public DataItem Child { get; set; }

        [BindProperty]
        public Guid ChildTemplateId { get; set; }

        [BindProperty]
        public Guid ParentId { get; set; }
        
        [BindProperty]
        public Guid ButtonId { get; set; }
        
        [BindProperty]
        public Guid NextStatus { get; set; }

        public EntityTemplate Template { get; set; }

        public NewChildItemModel(IAuthorizationService auth, IEntityTemplateService temp, IWorkflowService workf, ISubmissionService serv, AppDbContext db) : base(auth, serv)
        {
            _entityTemplateService = temp;
            _workflowService = workf;
            _db = db;
        }
        public void OnGet(Guid id, Guid childTemplateId, Guid buttonId)
        {
            ParentId = id;
            ChildTemplateId = childTemplateId;

            // Load the item with the specified "id"
            Item item = _submissionService.GetSubmissionDetails(id);

            // Get the entity template which has its ID to be the above loaded item's TemplateId
            Template = _entityTemplateService.GetTemplate(item.TemplateId.Value);

            // Get the data item that is referred by the given childTemplateId from the template
            Child = Template.GetDataItem(childTemplateId);

            ButtonId = buttonId;
        }
        public IActionResult OnPost()
        {
            //Creating a clone of the child entity


            // not a root entity??
            //EntityTemplate template = _entityTemplateService.GetTemplate(ChildTemplateId);
            //if (template == null)
            //    throw new Exception("Entity template with ID = " + ChildTemplateId + " not found.");

            // Get Parent Item to which Child will be added
            Item parentItem = _submissionService.GetSubmissionDetails(ParentId);
            if (parentItem == null)
                throw new Exception("Entity template with ID = " + ParentId + " not found.");

            //get template from parent
            EntityTemplate template = _entityTemplateService.GetTemplate(parentItem.TemplateId.Value);

            var postAction = _workflowService.GetPostActionByButtonId(template, ButtonId);
            var stateMapping = postAction.StateMappings.Where(sm => sm.Id == ButtonId).FirstOrDefault();
            var nextStatus = stateMapping.Next;

            User user = _workflowService.GetLoggedUser();
            //When we instantantiate an instance from the template, we do not need to clone metadata sets
            //Item newItem = template.Instantiate<Item>();
            parentItem.StatusId = nextStatus;
            parentItem.Updated = DateTime.Now;
            parentItem.AddAuditEntry(user.Id, stateMapping.Current, stateMapping.Next, stateMapping.ButtonLabel);

            // instantantiate a version of the child and update it

            DataItem newChildItem = template.InstantiateDataItem(this.Child.Id);
            newChildItem.UpdateFieldValues(this.Child);
            parentItem.DataContainer.Add(newChildItem);

            _db.Items.Update(parentItem);
            _db.SaveChanges();

            return RedirectToPage("ItemDetails", new { id = parentItem.Id });
        }
    }
}