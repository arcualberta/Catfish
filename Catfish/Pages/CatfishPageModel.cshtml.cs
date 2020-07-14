using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class CatfishPageModelModel : PageModel
    {
        protected IAuthorizationService AuthorizationSertvice { get; set; }
        public CatfishPageModelModel(IAuthorizationService auth)
        {
            AuthorizationSertvice = auth;
        }
    }
}