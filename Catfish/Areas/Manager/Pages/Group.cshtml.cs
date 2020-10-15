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

        [BindProperty]
        public List<GroupTemplateAssignmentVM> Templates { get; set; }

        
        public List<UserGroupRole> Users { get; set; }

        
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
                var groupTemplateVM = new GroupTemplateAssignmentVM()
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
                groupRoleVM.HasUsers = _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == groupRoleVM.RoleGroupId).Any();
                Roles.Add(groupRoleVM);
            }


            Users = _appDb.UserGroupRoles
                .Where(ugr => ugr.GroupRole.GroupId == id)
                .ToList();
            foreach (var user in Users)
                user.User = _srv.GetUserDetails(user.UserId);

        }

        public IActionResult OnPost()
        {
            Group group = SaveGroupRoles();
            SaveGroupTemplates(group);
            _appDb.SaveChanges();
           
            return RedirectToPage("GroupEdit","Manager", new { id = group.Id });
        }

        public List<GroupRoleAssignmentVM> GetGroupRoleList()
        {
            var roles = _srv.GetGroupRolesDetails();

            foreach (var role in roles)
            {
                var groupRoleVM = new GroupRoleAssignmentVM()
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
                var groupTemplateVM = new GroupTemplateAssignmentVM()
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

        public Group SaveGroupRoles()
        {
            //get group details from Groups table
            Group dbGroup = _srv.GetGroupDetails(Group.Id);

            if (dbGroup == null)
            {
                dbGroup = new Group();
            }

            dbGroup.Name = Group.Name;
            dbGroup.GroupStatus = Group.GroupStatus;
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
                    var newGroupRole = new GroupRole();
                    if (role.RoleGroupId == null)
                        newGroupRole.Id = Guid.NewGuid();
                    else
                        newGroupRole.Id = (Guid)role.RoleGroupId;

                    newGroupRole.RoleId = role.RoleId;
                    newGroupRole.Group = dbGroup;
                    newGroupRole.GroupId = dbGroup.Id;
                    selectedGroupRoles.Add(newGroupRole);
                }
            }
            //get all newly added roles to a list
            List<GroupRole> newlyAddedRoles = selectedGroupRoles.Except(dbGroupRoles, new GroupRoleComparer()).ToList();
            //get all deleted roles to a list
            List<GroupRole> deletedRoles = dbGroupRoles.Except(selectedGroupRoles, new GroupRoleComparer()).ToList();

            
            //add newly added roles to GroupRoles table
            if (newlyAddedRoles.Count > 0)
                foreach (var groupRole in newlyAddedRoles)
                    _appDb.GroupRoles.Add(groupRole);
            //remove deleted roles from GroupRoles table
            if (deletedRoles.Count > 0)
                foreach (var groupRole in deletedRoles)
                    _appDb.GroupRoles.Remove(groupRole);
            
            return dbGroup;

        }
        public void SaveGroupTemplates(Group dbGroup)
        {
            
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
                    var newGroupTemplate = new GroupTemplate();

                    if (template.TemplateGroupId == null)
                        newGroupTemplate.Id = Guid.NewGuid();
                    else
                        newGroupTemplate.Id = (Guid)template.TemplateGroupId;

                    newGroupTemplate.EntityTemplateId = template.TemplateId;
                    newGroupTemplate.Group = dbGroup;
                    newGroupTemplate.GroupId = dbGroup.Id;
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
