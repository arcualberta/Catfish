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

        public EntityEditPageModel(IAuthorizationService auth, ISubmissionService serv, IWorkflowService workflow) : base(auth, serv)
        {
            _workflowService = workflow;
        }
        public void OnGet()
        {


        }

        public void OnPost()
        {

        }
    }
}
