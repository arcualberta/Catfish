using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityDetailsPageModel : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;
        private readonly ISubmissionService _submissionService;
        private readonly AppDbContext _db;

        [BindProperty]
        public DataItem Item { get; set; }
        [BindProperty]
        public Guid ItemId { get; set; }
        public EntityDetailsPageModel(IAuthorizationService auth, ISubmissionService serv, IWorkflowService workflow, AppDbContext db) : base(auth, serv)
        {
            _workflowService = workflow;
            _submissionService = serv;
            _db = db;
        }
        public void OnGet(Guid id)
        {
            ItemId = id;
            
            
            Item item = _submissionService.GetSubmissionDetails(id);
            _workflowService.SetModel(item);

            //TODO: dynamically figure out the start-up item
            Item = item.GetRootDataItem(false);
        }
    }
}