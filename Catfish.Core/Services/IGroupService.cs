﻿using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public interface IGroupService
    {
        IList<Group> GetGroupList();
        IQueryable<User> GetUsers(int offset = 0, int max = 25);
        List<UserGroupRole> GetGroupRoleUser(Guid? id);
        User GetUserDetails(Guid userId);
        GroupRole GetGroupRoleDetails(Guid id);
        IList<Guid> GetAllUserIds(string searching);
        IList<Guid> GetGroupUserIds(Guid id);
        UserGroupRole AddUserGroupRole(Guid userId, Guid groupRoleId);
        List<GroupRoleUserAssignmentVM> SetUserAttribute(Guid groupRoleId, string searching);
        IList<Role> GetGroupRolesDetails();
        Group GetGroupDetails(Guid id);
        List<GroupTemplateAssignmentVM> SetTemplateAttribute(Guid groupId);
        List<GroupRoleAssignmentVM> SetRoleAttribute(Guid groupId);
        List<UserGroupRole> SetUserAttribute(Guid groupId);
        Group SaveGroupRoles(Group group, List<GroupRoleAssignmentVM> roles);
        void SaveGroupTemplates(Group group, List<GroupTemplateAssignmentVM> templates);
        void DeleteGroup(Guid groupId);
        bool CheckUserGroupRole(Guid groupId);
        void DeleteUserGroupRole(Guid userGroupRoleId);
    }
}
