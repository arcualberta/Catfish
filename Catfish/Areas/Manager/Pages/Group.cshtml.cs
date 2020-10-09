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
using Microsoft.EntityFrameworkCore;
using Piranha.AspNetCore.Identity.SQLServer;

namespace Catfish.Areas.Manager.Pages
{
    public class GroupModel : PageModel
    {
        private const string NotFound = "Group not found.";

        private IAuthorizationService _srv;
        public readonly AppDbContext _appDb;
        public readonly IdentitySQLServerDb _piranhaDb;

        [BindProperty]
        public Group Group { get; set; }

        [BindProperty]
        public List<GroupRoleAssignmentVM> Roles { get; set; }

        public List<GroupRoleAssignmentVM> RoleList { get; set; }

        [BindProperty]
        public List<GroupTemplateAssignmentVM> Templates { get; set; }

        [BindProperty]
        public List<UserGroupRole> Users { get; set; }

        //public  GroupModel()
        //{

        //}
        public GroupModel(IAuthorizationService srv, AppDbContext appDb, IdentitySQLServerDb pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }
        public void OnGet(Guid? id)
        {
            //If the id is given, retrieving the group from the database. Otherwise, creating a new one.
            if (id.HasValue)
                Group = _appDb.Groups.FirstOrDefault(g => g.Id == id);
            else
                Group = new Group()
                {
                    GroupStatus = Group.eGroupStatus.Inactive,
                    Id = Guid.NewGuid()
                };

            if (Group == null)
                throw new Exception(NotFound);

            //BEGIN: Handling entity templates
            //================================
            //Getting all templates available in the system and then creating a view model that identifies which of them
            //have been assigned to the current group
            var templates = _appDb.ItemTemplates.ToList(); //All templates in the system
            var groupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == id).ToList();

            Templates = new List<GroupTemplateAssignmentVM>();
            foreach (var template in templates)
            {
                var groupTemplateVM = new GroupTemplateAssignmentVM
                {
                    TemplateId = template.Id,
                    TemplateName = template.TemplateName
                };
                var currentAssociation = groupTemplates.Where(gt => gt.EntityTemplateId == template.Id).FirstOrDefault();
                groupTemplateVM.TemplateGroupId = currentAssociation == null ?  null as Guid?: currentAssociation.Id;
                groupTemplateVM.Assigned = groupTemplateVM.TemplateGroupId.HasValue;
                Templates.Add(groupTemplateVM);
            }
            //END: Handling entity templates

            //BEGIN: Handling roles
            //================================
            //Getting all roles available in the system and then creating a view model that identifies which of them
            //have been assigned to the current group
            var roles = _srv.GetGroupRolesDetails();
            var groupRoles = _appDb.GroupRoles.Where(gr => gr.GroupId == id).ToList();

            Roles = new List<GroupRoleAssignmentVM>();
            foreach (var role in roles)
            {
                var groupRoleVM = new GroupRoleAssignmentVM()
                {
                     RoleId = role.Id,
                      RoleName = role.Name,
                };
                var currentAssociation = groupRoles.Where(gr => gr.RoleId == role.Id).FirstOrDefault();
                groupRoleVM.RoleGroupId = currentAssociation == null ? null as Guid? : currentAssociation.Id;
                groupRoleVM.Assigned = groupRoleVM.RoleGroupId.HasValue;
                Roles.Add(groupRoleVM);
            }


            Users = _appDb.UserGroupRoles
                .Include(ugr => ugr.GroupRole)
                .Where(ugr => ugr.GroupId == id)
                .ToList();

            //Since Roles are in the Piranha DB Context, they cannot be included when UserGroupRoles are retrieved by
            //the above statement usin the AppDbContext, we need to assign them for each GroupRole.Role of each user 
            //as folllows.
            //foreach (var user in Users)
            //    user.GroupRole.Role = roles.Where(r => r.Id == user.GroupRole.RoleId).FirstOrDefault();


            
            ////RoleList = new List<GroupRoleAssignmentVM>();
            ////Users = new List<GroupRoleUserAssignmentVM>();
            //////var groupAdminRole = new GroupRoleAssignmentVM
            //////{
            //////    RoleId = groupAdmin.Id,
            //////    RoleName = groupAdmin.Name,
            //////    Assigned = true
            //////};
            //////RoleList.Add(groupAdminRole);
            ////foreach (var role in roles)
            ////{
            ////    var groupRoleVM = new GroupRoleAssignmentVM
            ////    {
            ////        RoleId = role.Id,
            ////        RoleName = role.Name
            ////    };
            ////    foreach (var groupRole in groupRoles)
            ////    {
            ////        if (role.Id == groupRole.RoleId)
            ////        {
            ////            groupRoleVM.Assigned = true;
            ////        }
            ////        groupRoleVM.RoleGroupId = groupRole.Id;
            ////    }
            ////    Roles.Add(groupRoleVM);
            ////    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
            ////}
            ////RoleList = Roles.OrderByDescending(r => r.Assigned).ToList();


            ////foreach (var user in users)
            ////{
            ////    var userGroupRolesVM = new GroupRoleUserAssignmentVM
            ////    {
            ////        UserId = user.Id,
            ////        UserName = user.UserName

            ////    };
            ////    foreach (var userGroupRole in userGroupRoles)
            ////    {
            ////        if (user.Id == userGroupRole.UserId)
            ////        {
            ////            userGroupRolesVM.Assigned = true;
            ////        }
            ////        userGroupRolesVM.RoleGroupId = userGroupRole.GroupRoleId;
            ////        userGroupRolesVM.GroupRoleUserId = userGroupRole.Id;
            ////    }
            ////    Users.Add(userGroupRolesVM);
            ////    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
            ////}
        }

        public IActionResult OnPost()
        {
            SaveGroupRoles();
            SaveGroupTemplates();
            _appDb.SaveChanges();

            return RedirectToPage("GroupEdit","Manager", Group.Id);
        }

        public List<GroupRoleAssignmentVM> GetGroupRoleList()
        {
            var roles = _srv.GetGroupRolesDetails();

            foreach (var role in roles)
            {
                var groupRoleVM = new GroupRoleAssignmentVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Assigned = false
                };

                Roles.Add(groupRoleVM);

            }
            return Roles;
        }

        public List<GroupTemplateAssignmentVM> GetGroupTemplateList()
        {
            var templates = _appDb.ItemTemplates.ToList();

            foreach (var template in templates)
            {
                var groupTemplateVM = new GroupTemplateAssignmentVM
                {
                    TemplateId = template.Id,
                    TemplateName = template.TemplateName,
                    Assigned = false
                };

                Templates.Add(groupTemplateVM);
                //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
            }
            return Templates;
        }

        public void SaveGroupRoles()
        {
            //get group details from Groups table
            Group dbGroup = _srv.GetGroupDetails(Group.Id);
            //get group roles details from GroupRoles table
            List<GroupRole> dbGroupRoles = _appDb.GroupRoles.Where(r => r.GroupId == Group.Id).ToList();
            //get roles associate data from interface 
            List<GroupRoleAssignmentVM> newList = Roles;
            List<GroupRole> selectedGroupRoles = new List<GroupRole>();
            //get all selected roles list
            foreach (var role in newList)
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

        }
        public void SaveGroupTemplates()
        {
            //get group details from Groups table
            Group dbGroup = _srv.GetGroupDetails(Group.Id);
            //get group template details from GroupTemplates table
            List<GroupTemplate> dbGroupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == Group.Id).ToList();
            //get templates associate data from interface 
            List<GroupTemplateAssignmentVM> newList = Templates;
            List<GroupTemplate> selectedGroupTemplates = new List<GroupTemplate>();
            //get all selected template list
            foreach (var template in newList)
            {
                if (template.Assigned)
                {
                    var newGroupTemplate = new GroupTemplate
                    {
                        Id = Guid.NewGuid(),
                        EntityTemplateId = template.TemplateId,
                        Group = dbGroup,
                        GroupId = dbGroup.Id
                    };
                    selectedGroupTemplates.Add(newGroupTemplate);
                }
            }
            //get all newly added templates to a list
            List<GroupTemplate> newlyAddedTemplates = selectedGroupTemplates.Except(dbGroupTemplates, new GroupTemplateComparer()).ToList();
            //get all deleted templates to a list
            List<GroupTemplate> deletedTemplates = dbGroupTemplates.Except(selectedGroupTemplates, new GroupTemplateComparer()).ToList();

            if (dbGroup == null)
                throw new Exception("Group Details with ID = " + Group.Id + " not found.");
            //add newly added templates to GroupTemplates table
            if (newlyAddedTemplates.Count > 0)
                foreach (var groupTemplate in newlyAddedTemplates)
                    _appDb.GroupTemplates.Add(groupTemplate);
            //remove deleted roles from GroupTemplates table
            if (deletedTemplates.Count > 0)
                foreach (var groupTemplate in deletedTemplates)
                    _appDb.GroupTemplates.Remove(groupTemplate);

        }

    }
}
