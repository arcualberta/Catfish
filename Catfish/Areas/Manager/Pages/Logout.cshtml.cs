using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha;

namespace Catfish.Areas.Manager.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly Piranha.AspNetCore.Identity.IDb _db;
     
        private readonly ICatfishAppConfiguration _catfishConfig;
        private readonly ISecurity _security;

        public LogoutModel( Piranha.AspNetCore.Identity.IDb db, ICatfishAppConfiguration catfishConfig, ISecurity security) : base()
        {
            _db = db;  
            _catfishConfig = catfishConfig;
            _security = security;
        }
        public IActionResult OnGet()
        {

            string logoutUrl = _catfishConfig.GetValue("SiteConfig:LogoutRedirectUrl", "/manager");
            _security.SignOut(_db);
            return Redirect(logoutUrl);
        }
       

    }
}
