using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using Catfish.Core.Services;
using Catfish.Models.ViewModels;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.SQLServer;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupUserAddModel : PageModel
    {
        private IGroupService _srv;
        public readonly AppDbContext _appDb;
        public readonly IdentitySQLServerDb _piranhaDb;

        [BindProperty]
        public GroupRole GroupRole { get; set; }

        [BindProperty]
        public List<GroupRoleUserAssignmentVM> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Searching { get; set; }

        public GroupUserAddModel(IGroupService srv, AppDbContext appDb, IdentitySQLServerDb pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }

        public async Task OnGetAsync(Guid id)
        {
            //get selected group role details
            GroupRole = _srv.GetGroupRoleDetails(id);
            Users = _srv.SetUserAttribute(GroupRole.Id, Searching);
        }
        public IActionResult OnPost()
        {
            GroupRole = _srv.GetGroupRoleDetails(GroupRole.Id);
            foreach (var userGroupRole in Users.Where(ugr => ugr.Assigned))
                _srv.AddUserGroupRole(userGroupRole.UserId, userGroupRole.RoleGroupId);

            _appDb.SaveChanges();
            return RedirectToPage("GroupEdit", "Manager", new { id=GroupRole.GroupId });
            
        }
    }
}
