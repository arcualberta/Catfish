using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using Catfish.Core.Services;
using Catfish.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;

namespace Catfish.Areas.Manager.Pages
{
    public class AddTemplateCollectionModel : PageModel
    {
        private IGroupService _srv;
        public readonly AppDbContext _appDb;
        public readonly IdentitySQLServerDb _piranhaDb;
        private readonly UserManager<User> _userManager;
        private readonly ICatfishAppConfiguration _catfishConfig;

        [BindProperty]
        public GroupTemplate GroupTemplate { get; set; }

        [BindProperty]
        public List<TemplateCollectionVM> Collections { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Searching { get; set; }

        [BindProperty]
        public string AdditinalUsers { get; set; }
        
        public void OnGet()
        {
        }
    }
}
