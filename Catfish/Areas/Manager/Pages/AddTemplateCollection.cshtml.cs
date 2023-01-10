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

        
        public AddTemplateCollectionModel(IGroupService srv, AppDbContext appDb, IdentitySQLServerDb pdb, UserManager<User> userManager, ICatfishAppConfiguration catfishConfig)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
            _userManager = userManager;
            _catfishConfig = catfishConfig;

        }
        public async Task OnGetAsync(Guid id)
        {
            GroupTemplate = _srv.GetGroupTemplateDetails(id);
            Collections = _srv.SetTemplateCollectionAttribute(id);
        }

        public IActionResult OnPost()
        {
            GroupTemplate = _srv.GetGroupTemplateDetails(GroupTemplate.Id);
            foreach (var collection in Collections.Where(ugr => ugr.Assigned))
                _srv.AddTemplateCollections(GroupTemplate.Id, collection.CollectionId);

            _appDb.SaveChanges();
            return RedirectToPage("GroupEdit", "Manager", new { id = GroupTemplate.GroupId });

        }
    }
}
