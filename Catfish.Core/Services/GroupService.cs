using Catfish.Core.Models;
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

        
    }
}
