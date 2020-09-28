using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private IAuthorizationService _srv;
        public readonly AppDbContext _appDb;

        [BindProperty]
        public Group Group { get; set; }

        public GroupModel(IAuthorizationService srv, AppDbContext appDb)
        {
            _srv = srv;
            _appDb = appDb;
        }
        public void OnGet(Guid id)
        {
            Group = _srv.GetGroupDetails(id);
        }

        public IActionResult OnPost()
        {
            Group dbGroup = _srv.GetGroupDetails(Group.Id);
            if (dbGroup == null)
                throw new Exception("Group Details with ID = " + Group.Id + " not found.");

            dbGroup.Name = Group.Name;
            dbGroup.GroupStatus = Group.GroupStatus;
            _appDb.SaveChanges();

            return RedirectToPage("GroupEdit","Manager");
        }

    }
}
