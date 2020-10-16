using Catfish.Core.Models;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public interface IUserGroupService
    {
        IList<Group> GetGroupList();
        IQueryable<User> GetUsers(int offset = 0, int max = 25);
        List<UserGroupRole> GetGroupRoleUser(Guid? id);
        User GetUserDetails(Guid userId);
        GroupRole GetGroupRoleDetails(Guid id);
        void EnsureUserRoles(Guid userId, Guid roleId);
        IList<Guid> GetAllUserIds(string searching);
        IList<Guid> GetGroupUserIds(Guid id);
        
    }
}
