using Catfish.Core.Models;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access.AuthorizationHandlers
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

        public bool HasRoleInGroup(Guid userId, List<Guid> targetGroupIds, List<Guid> targetRoleIds)
        {
            throw new NotImplementedException();
        }
    }
}
