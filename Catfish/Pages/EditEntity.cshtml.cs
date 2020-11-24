using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityEditPageModel : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;
        private readonly ISubmissionService _submissionService;
        private readonly AppDbContext _db;

        [BindProperty]
        public DataItem Item { get; set; }

        public EntityEditPageModel(IAuthorizationService auth, ISubmissionService serv, IWorkflowService workflow, AppDbContext db) : base(auth, serv)
        {
            _workflowService = workflow;
            _submissionService = serv;
            _db = db;
        }
        public void OnGet(Guid id)
        {
            Item item = _submissionService.GetSubmissionDetails(id);

            Item = item.GetRootDataItem(false);
        }

        public IActionResult OnPost()
        {
            //Loading the entity from the database using the EntityId
            Item item = _db.Items.Where(it => it.Id == Item.EntityId).FirstOrDefault();
            if (item == null)
                return NotFound("Requested item not found.");

            DataItem dbDataItem = item.DataContainer.Where(it => it.Id == Item.Id).FirstOrDefault();
            if (dbDataItem == null)
                return NotFound("Requested data object not found.");

            dbDataItem.UpdateFieldValues(Item);
            item.Updated = DateTime.Now;

            _db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToPage("EntityDetailsPage", new { id = item.Id });
        }
    }
}