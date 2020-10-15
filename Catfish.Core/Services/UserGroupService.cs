using Catfish.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public class UserGroupService: IUserGroupService
    {
        private readonly AppDbContext _appDb;
        private readonly IdentitySQLServerDb _piranhaDb;
        public UserGroupService(AppDbContext db, IdentitySQLServerDb pdb )
        {
            _appDb = db;
            _piranhaDb = pdb;
        }

        public IList<Group> GetGroupList()
        {
            return _appDb.Groups.ToList();
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

    }
}
