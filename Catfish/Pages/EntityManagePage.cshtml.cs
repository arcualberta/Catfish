using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityManagePageModel : CatfishPageModelModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ISubmissionService _submissionService;
        public EntityManagePageModel(IAuthorizationService auth, ISubmissionService serv) : base(auth, serv)
        {
            _authorizationService = auth;
            _submissionService = serv;
        }
        public void OnGet()
        {

        }
    }
}