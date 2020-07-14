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
        public StartPageModel(IAuthorizationService auth) : base(auth)
        {
        }

        public void OnGet()
        {

        }
    }
}