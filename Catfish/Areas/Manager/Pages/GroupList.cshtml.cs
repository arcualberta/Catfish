using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupListModel : PageModel
    {
        public readonly AppDbContext _appDb;
        public GroupService _srv;

        public IList<Group> Groups { get; set; }
        

        public GroupListModel(GroupService srv, AppDbContext appDb)
        {
            _srv = srv;
            _appDb = appDb;
        }
        public void OnGet()
        {
            Groups  = _srv.GetGroupList();
        }
        public IActionResult OnPost(Guid groupId)
        {
            _srv.DeleteGroup(groupId);
            _appDb.SaveChanges();
            return RedirectToPage("GroupList", "Manager");
        }

    }
}
