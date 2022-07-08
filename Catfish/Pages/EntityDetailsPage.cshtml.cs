using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
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

        [BindProperty]
        public Guid? ChildTemplateId { get; set; }

        public EntityDetailsPageModel(IAuthorizationService auth, ISubmissionService serv, IWorkflowService workflow, AppDbContext db) : base(auth, serv)
        {
            _workflowService = workflow;
            _submissionService = serv;
            _db = db;
        }
        public void OnGet(Guid id)
        {
            ItemId = id;
            
            Item entity = _submissionService.GetSubmissionDetails(id);
            _workflowService.SetModel(entity);

            Item = entity.GetRootDataItem(false);

            //Temporarily selecting the first non-root data-item template from the data container as the 
            //form to be used for adding comments to the current item identified by the input GUILd "id"
            EntityTemplate template = _workflowService.GetTemplate();
            ChildTemplateId = template.DataContainer
                .Where(dataItem => dataItem.IsRoot == false)
                .Select(dataItem => dataItem.Id)
                .FirstOrDefault();
        }
    }
}