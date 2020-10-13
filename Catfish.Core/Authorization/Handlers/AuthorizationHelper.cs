using Catfish.Core.Models;
using Microsoft.EntityFrameworkCore;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class AuthorizationHelper: IAuthorizationHelper
    {
        public readonly AppDbContext _appDbContext;
        public readonly IdentitySQLServerDb _piranhaDbContext;
        public AuthorizationHelper(AppDbContext adb, IdentitySQLServerDb pdb)
        {
            _appDbContext = adb;
            _piranhaDbContext = pdb;
        }

        public IQueryable<Guid> GetGroupsAssociatedWithTemplate(Guid entityTemplateId)
        {
            return _appDbContext.GroupTemplates
                .Where(gt => gt.EntityTemplateId == entityTemplateId)
                .Select(gt => gt.GroupId);
        }

        /// <summary>
        /// Returns true if the user idenfied by the userId has at least one role in the targetRoleIds
        /// within at least one of the groups identified by the targetGroupIds
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="targetGroupIds"></param>
        /// <param name="targetRoleIds"></param>
        /// <returns></returns>
        public bool HasRoleInGroup(Guid userId, List<Guid> targetGroupIds, List<Guid> targetRoleIds)
        {
            var hasRoleInGroup = _appDbContext.UserGroupRoles.Include(ugr => ugr.GroupRole)
                .Where(ugr => ugr.UserId == userId
                       && targetGroupIds.Contains(ugr.GroupRole.GroupId)
                       && targetRoleIds.Contains(ugr.GroupRole.RoleId)
                       )
                .Any();

            return hasRoleInGroup;
        }
    }
}
