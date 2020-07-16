using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class StartPageModel : CatfishPageModelModel
    {
        protected readonly IEntityTemplateService _entityTemplateService;
        public StartPageModel(IAuthorizationService auth, ISubmissionService serv, IEntityTemplateService temp)
            : base(auth, serv)
        {
            _entityTemplateService = temp;
        }

        public void OnGet()
        {

        }
    }
}