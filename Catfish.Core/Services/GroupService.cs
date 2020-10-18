using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.Core.Services
{
    public class GroupService: IGroupService
    {
        private readonly AppDbContext _appDb;
        private readonly IdentitySQLServerDb _piranhaDb;
        public GroupService(AppDbContext db, IdentitySQLServerDb pdb)
        {
            _appDb = db;
            _piranhaDb = pdb;
        }

        public IList<Group> GetGroupList()
        {
            return _appDb.Groups.Where(gr => gr.GroupStatus != Group.eGroupStatus.Deleted).ToList();
        }

        public IQueryable<User> GetUsers(int offset = 0, int max = 25)
        {
            return _piranhaDb.Users.OrderBy(u => u.UserName).Skip(offset).Take(max);
        }

        public IQueryable<User> GetUsers(Guid groupRoleId, int offset = 0, int max = 25)
        {
            var usersWithGivenRoleInGroup = _appDb.UserGroupRoles
                .Where(ugr => ugr.GroupRoleId == groupRoleId)
                .Select(ugr => ugr.UserId)
                .ToList();

            return _piranhaDb.Users.Where(u => usersWithGivenRoleInGroup.Contains(u.Id));
        }

        public List<UserGroupRole> GetGroupRoleUser(Guid? id)
        {
            List<UserGroupRole> Users = new List<UserGroupRole>();
            Users = _appDb.UserGroupRoles
                .Where(ugr => ugr.GroupRoleId == id)
                .ToList();
            foreach (var user in Users)
                user.User = GetUserDetails(user.UserId);
            return Users;
        }

        public User GetUserDetails(Guid userId)
        {
            return _piranhaDb.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public GroupRole GetGroupRoleDetails(Guid id)
        {
            return _appDb.GroupRoles.Where(gr => gr.Id == id).FirstOrDefault();
        }

        public UserGroupRole AddUserGroupRole(Guid userId, Guid groupRoleId)
        {
            UserGroupRole ugr = new UserGroupRole()
            {
                Id = Guid.NewGuid(),
                GroupRoleId = groupRoleId,
                UserId = userId,
            };
            _appDb.UserGroupRoles.Add(ugr);

            return ugr;
        }

        public IList<Guid> GetAllUserIds(string searching)
        {
            return _piranhaDb.Users.Where(usr => usr.Email.Contains(searching) || searching == null).Select(ur => ur.Id).ToList();
        }

        public IList<Guid> GetGroupUserIds(Guid id)
        {
            return _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == id).Select(ugr => ugr.UserId).ToList();
        }

        public List<GroupRoleUserAssignmentVM> SetUserAttribute(Guid groupRoleId, string searching)
        {
            //get all users who have the selected role
            var allRoleUsers = GetAllUserIds(searching);
            //get userId's who already selected for perticular user group
            var addedRoleUsers = GetGroupUserIds(groupRoleId);

            //get userId's who doesn't selected to a perticular role group
            var toBeAddedRoleUsers = allRoleUsers.Except(addedRoleUsers).ToList();

            //get all user details
            var users = GetUsers();
            List<GroupRoleUserAssignmentVM> Users = new List<GroupRoleUserAssignmentVM>();
            foreach (var newUser in toBeAddedRoleUsers)
            {
                var user = users.Where(u => u.Id == newUser).FirstOrDefault();
                var groupRoleUserAssignmentVM = new GroupRoleUserAssignmentVM()
                {
                    UserId = user.Id,
                    RoleGroupId = groupRoleId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Assigned = false
                };
                Users.Add(groupRoleUserAssignmentVM);
            }
            return Users;
        }

        public IList<Role> GetGroupRolesDetails()
        {
            return _piranhaDb.Roles.Where(r => r.NormalizedName != "SYSADMIN").OrderBy(r => r.Name).ToList();
        }

        public Group GetGroupDetails(Guid id)
        {

            return _appDb.Groups.Where(gr => gr.Id == id).FirstOrDefault();
        }

        public List<GroupTemplateAssignmentVM> SetTemplateAttribute(Guid groupId)
        {
            //BEGIN: Handling entity templates
            //================================
            //Getting all templates available in the system and then creating a view model that identifies which of them
            //have been assigned to the current group
            var templates = _appDb.ItemTemplates.ToList(); //All templates in the system
            var groupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == groupId).ToList();

            List<GroupTemplateAssignmentVM> templatesList = new List<GroupTemplateAssignmentVM>();
            foreach (var template in templates)
            {
                var groupTemplateVM = new GroupTemplateAssignmentVM()
                {
                    TemplateId = template.Id,
                    TemplateName = template.TemplateName
                };
                var currentAssociation = groupTemplates.Where(gt => gt.EntityTemplateId == template.Id).FirstOrDefault();
                groupTemplateVM.TemplateGroupId = currentAssociation == null ? null as Guid? : currentAssociation.Id;
                groupTemplateVM.Assigned = groupTemplateVM.TemplateGroupId.HasValue;
                templatesList.Add(groupTemplateVM);
            }
            //END: Handling entity templates
            return templatesList;
        }

        public List<GroupRoleAssignmentVM> SetRoleAttribute(Guid groupId)
        {
            //BEGIN: Handling roles
            //================================
            //Getting all roles available in the system and then creating a view model that identifies which of them
            //have been assigned to the current group
            var roles = GetGroupRolesDetails();
            var groupRoles = _appDb.GroupRoles.Where(gr => gr.GroupId == groupId).ToList();

            List<GroupRoleAssignmentVM>  roleList = new List<GroupRoleAssignmentVM>();
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
                roleList.Add(groupRoleVM);
            }
            return roleList;
        }

        public List<UserGroupRole> SetUserAttribute(Guid groupId)
        {
            List<UserGroupRole> users = _appDb.UserGroupRoles
                .Where(ugr => ugr.GroupRole.GroupId == groupId)
                .ToList();
            foreach (var user in users)
                user.User = GetUserDetails(user.UserId);
            return users;
        }

        public Group SaveGroupRoles(Group group, List<GroupRoleAssignmentVM> roles)
        {
            //get group details from Groups table
            Group dbGroup = GetGroupDetails(group.Id);

            if (dbGroup == null)
            {
                dbGroup = new Group();
            }

            dbGroup.Name = group.Name;
            dbGroup.GroupStatus = group.GroupStatus;
            //get group roles details from GroupRoles table
            List<GroupRole> dbGroupRoles = _appDb.GroupRoles.Where(r => r.GroupId == group.Id).ToList();
            //get roles associate data from interface 
            List<GroupRoleAssignmentVM> newList = roles;
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
        public void SaveGroupTemplates(Group group, List<GroupTemplateAssignmentVM> templates)
        {

            //get group template details from GroupTemplates table
            List<GroupTemplate> dbGroupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == group.Id).ToList();
            //get templates associate data from interface 
            List<GroupTemplateAssignmentVM> newList = templates;
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
                    newGroupTemplate.Group = group;
                    newGroupTemplate.GroupId = group.Id;
                    selectedGroupTemplates.Add(newGroupTemplate);
                }
            }
            //get all newly added templates to a list
            List<GroupTemplate> newlyAddedTemplates = selectedGroupTemplates.Except(dbGroupTemplates, new GroupTemplateComparer()).ToList();
            //get all deleted templates to a list
            List<GroupTemplate> deletedTemplates = dbGroupTemplates.Except(selectedGroupTemplates, new GroupTemplateComparer()).ToList();

            if (group == null)
                throw new Exception("Group Details with ID = " + group.Id + " not found.");
            //add newly added templates to GroupTemplates table
            if (newlyAddedTemplates.Count > 0)
                foreach (var groupTemplate in newlyAddedTemplates)
                    _appDb.GroupTemplates.Add(groupTemplate);
            //remove deleted roles from GroupTemplates table
            if (deletedTemplates.Count > 0)
                foreach (var groupTemplate in deletedTemplates)
                    _appDb.GroupTemplates.Remove(groupTemplate);
        }

        public void DeleteGroup(Guid groupId)
        {
            Group group = GetGroupDetails(groupId);
            group.GroupStatus = Group.eGroupStatus.Deleted;
            _appDb.Groups.Update(group);
        }

        public bool CheckUserGroupRole(Guid groupId)
        {
            var groupRoles = _appDb.GroupRoles.Where(gr => gr.GroupId == groupId);
            if (groupRoles == null)
                return false;
            else
            {
                foreach(var groupRole in groupRoles)
                {
                    var userGroupRole = _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == groupRole.Id);
                    if (userGroupRole.Any())
                        return true;
                }
                return false;
            }
            
        }
    }
}
