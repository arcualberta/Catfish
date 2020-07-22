using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class CreateChildPageModel : CatfishPageModelModel
    {
        public CreateChildPageModel(IAuthorizationService auth, ISubmissionService serv) : base(auth, serv)
        {
        }
        public void OnGet()
        {

        }
    }
}