using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityManagePageModel : CatfishPageModelModel
    {
        public EntityManagePageModel(IAuthorizationService auth, ISubmissionService serv, IEntityTemplateService temp) : base(auth, serv,temp)
        {
        }
        public void OnGet()
        {

        }
    }
}