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
            //get all users who have the selected role
            var allRoleUsers = _srv.GetAllUserIds(Searching);
            //get userId's who already selected for perticular user group
            var addedRoleUsers = _srv.GetGroupUserIds(GroupRole.Id);

            //get userId's who doesn't selected to a perticular role group
            var toBeAddedRoleUsers = allRoleUsers.Except(addedRoleUsers).ToList();

            //get all user details
            var users = _srv.GetUsers();
            Users = new List<GroupRoleUserAssignmentVM>();
            foreach (var newUser in toBeAddedRoleUsers)
            {
                var user = users.Where(u => u.Id == newUser).FirstOrDefault();
                var groupRoleUserAssignmentVM = new GroupRoleUserAssignmentVM()
                {
                    UserId = user.Id,
                    RoleGroupId = GroupRole.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Assigned = false
                };
                Users.Add(groupRoleUserAssignmentVM);
            }
        }
        public IActionResult OnPost()
        {
            foreach (var userGroupRole in Users.Where(ugr => ugr.Assigned))
                _srv.AddUserGroupRole(userGroupRole.UserId, userGroupRole.RoleGroupId);

            _appDb.SaveChanges();
            return RedirectToPage("GroupEdit", "Manager", new { id=GroupRole.GroupId });
            
        }
    }
}
