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
    public class CatfishPageModelModel : PageModel
    {
        protected readonly IAuthorizationService _authorizationSertvice;
        protected readonly ISubmissionService _submissionService;
        public CatfishPageModelModel(IAuthorizationService auth, ISubmissionService serv)
        {
            _authorizationSertvice = auth;
            _submissionService = serv;
        }
    }
}