using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityEditPageModel : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;
        private readonly AppDbContext _db;

        [BindProperty]
        public DataItem Item { get; set; }
        public EntityEditPageModel(IAuthorizationService auth, ISubmissionService serv, IWorkflowService workflow, AppDbContext db) : base(auth, serv)
        {
            _workflowService = workflow;
            _db = db;
        }
        public void OnGet(Guid id)
        {
            Entity Item = _db.Items.Where(it => it.Id == id).FirstOrDefault();

        }

        public void OnPost()
        {

        }
    }
}
