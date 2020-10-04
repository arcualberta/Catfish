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

        public List<GroupRoleAssignmentVM> RoleList { get; set; }

        [BindProperty]
        public List<GroupTemplateAssignmentVM> Templates { get; set; }

        //public  GroupModel()
        //{

        //}
        public GroupModel(IAuthorizationService srv, AppDbContext appDb, PiranhaDbContext pdb)
        {
            _srv = srv;
            _appDb = appDb;
            _piranhaDb = pdb;
        }
        public void OnGet(Guid id)
        {
            var group = _appDb.Groups.FirstOrDefault(g => g.Id == id);
            var roleNames = _srv.GetGroupRolesDetails();
            if (group != null)
            {
                Group = group;

                var roles = _srv.GetGroupRolesDetails();
                var groupAdmin = _piranhaDb.Roles.Where(r => r.NormalizedName == "GROUPADMIN").FirstOrDefault();
                var templates = _appDb.ItemTemplates.ToList();
                var groupRoles = _appDb.GroupRoles.Where(r => r.GroupId == id).ToList();
                var groupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == id).ToList();

                Roles = new List<GroupRoleAssignmentVM>();
                Templates = new List<GroupTemplateAssignmentVM>();
                RoleList = new List<GroupRoleAssignmentVM>();
                var groupAdminRole = new GroupRoleAssignmentVM
                {
                    RoleId = groupAdmin.Id,
                    RoleName = groupAdmin.Name,
                    Assigned = true
                };
                RoleList.Add(groupAdminRole);
                foreach (var role in roles)
                {
                    var groupRoleVM = new GroupRoleAssignmentVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    foreach (var groupRole in groupRoles)
                    {
                        if (role.Id == groupRole.RoleId)
                        {
                            groupRoleVM.Assigned = true;
                        }
                        groupRoleVM.RoleGroupId = groupRole.Id;
                    }
                    Roles.Add(groupRoleVM);
                    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
                }
                RoleList = Roles.OrderByDescending(r => r.Assigned).ToList();
                foreach (var template in templates)
                {
                    var groupTemplateVM = new GroupTemplateAssignmentVM
                    {
                        TemplateId = template.Id,
                        TemplateName = template.TemplateName
                    };
                    foreach (var groupTemplate in groupTemplates)
                    {
                        if (template.Id == groupTemplate.EntityTemplateId)
                        {
                            groupTemplateVM.Assigned = true;
                        }
                        groupTemplateVM.TemplateGroupId = groupTemplate.Id;
                    }
                    Templates.Add(groupTemplateVM);
                    //SelectedRoles.Add(Roles.Single(r => r.Id == role.RoleId).Name);
                }
            }
            //else 
            //{
            //    GroupModel groupModel = new GroupModel();
            //    groupModel.Create();
            //}


        }

        public IActionResult OnPost()
        {
            SaveGroupRoles();
            SaveGroupTemplates();
            _appDb.SaveChanges();

            return RedirectToPage("GroupList","Manager");
        }
        //public GroupModel Create()
        //{
        //    var roles = _srv.GetGroupRolesDetails();

        //    return new GroupModel
        //    {
        //        Group = new Group(),
        //        Roles = GetGroupRoleList(),
        //        Templates = GetGroupTemplateList()
        //    };
        //}

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
