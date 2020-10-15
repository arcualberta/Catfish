using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Models.ViewModels;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.SQLServer;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupUserAddModel : PageModel
    {
        private IAuthorizationService _srv;
        public readonly AppDbContext _appDb;
        public readonly IdentitySQLServerDb _piranhaDb;

        [BindProperty]
        public GroupRole GroupRole { get; set; }

        [BindProperty]
        public List<GroupRoleUserAssignmentVM> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Searching { get; set; }

        public GroupUserAddModel(IAuthorizationService srv, AppDbContext appDb, IdentitySQLServerDb pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }

        public async Task OnGetAsync(Guid? id)
        {
            //get selected group role details
            GroupRole = _appDb.GroupRoles.Where(gr => gr.Id == id).FirstOrDefault();
            //get all users who have the selected role
            var allRoleUsers = _piranhaDb.Users.Where(usr => usr.Email.Contains(Searching) || Searching == null).Select(ur => ur.Id).ToList();
            //get userId's who already selected for perticular user group
            var addedRoleUsers = _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == GroupRole.Id).Select(ugr => ugr.UserId).ToList();

            //get userId's who doesn't selected to a perticular role group
            var toBeAddedRoleUsers = allRoleUsers.Except(addedRoleUsers).ToList();

            //get all user details
            var users = _piranhaDb.Users.ToList();
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
            GroupRole = _appDb.GroupRoles.Where(gr => gr.Id == GroupRole.Id).FirstOrDefault();
            
            
            foreach(var userGroupRole in Users)
            {
                UserGroupRole dbUserGroupRole = new UserGroupRole();

                if (userGroupRole.Assigned)
                {
                    dbUserGroupRole.Id = Guid.NewGuid();
                    dbUserGroupRole.GroupRoleId = GroupRole.Id;
                    dbUserGroupRole.UserId = userGroupRole.UserId;

                    _appDb.UserGroupRoles.Add(dbUserGroupRole);
                }
                
            }

            _appDb.SaveChanges();
            return RedirectToPage("GroupEdit", "Manager", new { id=GroupRole.GroupId });
            
        }
    }
}
