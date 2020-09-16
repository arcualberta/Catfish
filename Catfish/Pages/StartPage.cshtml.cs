using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class StartPageModel : CatfishPageModelModel
    {
        //protected readonly IAuthorizationService _authorizationService;
        public IList<ItemTemplate> ItemTemplates { get; set; }
        //public StartPageModel(IAuthorizationService auth, ISubmissionService serv, IEntityTemplateService temp)
        //    : base(auth, serv)
        public StartPageModel(IAuthorizationService authorizationService)
            : base(authorizationService,null)
        {
            
        }

        public void OnGet()
        {
            ItemTemplates = _authorizationSertvice.GetSubmissionTemplateList();
        }
    }
}