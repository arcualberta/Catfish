using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Models.ViewModels;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AspNetCore.Identity.Data;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private IAuthorizationService _srv;
        public readonly AppDbContext _appDb;
        public readonly PiranhaDbContext _piranhaDb;

        [BindProperty]
        public Group Group { get; set; }

        [BindProperty]
        public List<AssignedRoleDataViewModel> Roles { get; set; }
        

        public GroupModel(IAuthorizationService srv, AppDbContext appDb, PiranhaDbContext pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }
        public void OnGet(Guid id)
        {
            var group = _appDb.Groups.FirstOrDefault(g => g.Id == id);
            
            if (group != null)
            {
                Group = group;
                
                var roles = _piranhaDb.Roles.Where(r => r.NormalizedName != "SYSADMIN").OrderBy(r => r.Name).ToList();
                
                var groupRoles = _appDb.GroupRoles.Where(r => r.GroupId == id).ToList();
                Roles = new List<AssignedRoleDataViewModel>();
                foreach (var role in roles)
                {
                    var groupRoleVM = new AssignedRoleDataViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    foreach (var groupRole in groupRoles)
                    {
                        if(role.Id== groupRole.RoleId)
                        {
                            groupRoleVM.Assigned = true;
                        }
                    }
                    Roles.Add(groupRoleVM);
                    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
                }              
            }
        }

        public IActionResult OnPost()
        {
            Group dbGroup = _srv.GetGroupDetails(Group.Id);
            List<GroupRole> dbGroupRoles = _appDb.GroupRoles.Where(r => r.GroupId == Group.Id).ToList();
            List<AssignedRoleDataViewModel> newList = Roles;

            if (dbGroup == null)
                throw new Exception("Group Details with ID = " + Group.Id + " not found.");

            dbGroup.Name = Group.Name;
            dbGroup.GroupStatus = Group.GroupStatus;
            _appDb.SaveChanges();

            return RedirectToPage("GroupList","Manager");
        }

    }
}
