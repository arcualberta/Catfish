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
        public List<GroupRoleAssignmentVM> Roles { get; set; }
        

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
                Roles = new List<GroupRoleAssignmentVM>();
                foreach (var role in roles)
                {
                    var groupRoleVM = new GroupRoleAssignmentVM
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
                        groupRoleVM.RoleGroupId = groupRole.Id;
                    }
                    Roles.Add(groupRoleVM);
                    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
                }              
            }
        }

        public IActionResult OnPost()
        {
            //get group details from Groups table
            Group dbGroup = _srv.GetGroupDetails(Group.Id);
            //get group roles details from GroupRoles table
            List<GroupRole> dbGroupRoles = _appDb.GroupRoles.Where(r => r.GroupId == Group.Id).ToList();
            //get roles associate data from interface 
            List<GroupRoleAssignmentVM> newList = Roles;
            List<GroupRole> selectedGroupRoles = new List<GroupRole>();
            //get all selected roles list
            foreach(var role in newList)
            {
                if (role.Assigned)
                {
                    var newGroupRole = new GroupRole
                    {
                        Id = Guid.NewGuid(),
                        RoleId = role.RoleId,
                        Group = dbGroup,
                        GroupId = dbGroup.Id
                    };
                    selectedGroupRoles.Add(newGroupRole);
                }
            }
            //get all newly added roles to a list
            List<GroupRole> newlyAddedRoles = selectedGroupRoles.Except(dbGroupRoles, new GroupRoleComparer()).ToList();
            //get all deleted roles to a list
            List<GroupRole> deletedRoles = dbGroupRoles.Except(selectedGroupRoles, new GroupRoleComparer()).ToList();
            
            if (dbGroup == null)
                throw new Exception("Group Details with ID = " + Group.Id + " not found.");

            dbGroup.Name = Group.Name;
            dbGroup.GroupStatus = Group.GroupStatus;
            //add newly added roles to GroupRoles table
            if (newlyAddedRoles.Count > 0)
                foreach (var groupRole in newlyAddedRoles)
                    _appDb.GroupRoles.Add(groupRole);
            //remove deleted roles from GroupRoles table
            if (deletedRoles.Count > 0)
                foreach (var groupRole in deletedRoles)
                    _appDb.GroupRoles.Remove(groupRole);
            _appDb.SaveChanges();

            return RedirectToPage("GroupList","Manager");
        }

    }
}
