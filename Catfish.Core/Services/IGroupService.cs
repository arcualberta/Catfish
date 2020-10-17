using Catfish.Core.Models;
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
    }
}
