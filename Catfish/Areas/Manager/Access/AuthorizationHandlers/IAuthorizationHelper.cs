using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access.AuthorizationHandlers
{
    public interface IAuthorizationHelper
    {
        public IQueryable<Guid> GetGroupsAssociatedWithTemplate(Guid entityTemplateId);
        public bool HasRoleInGroup(Guid userId, List<Guid> targetGroupIds, List<Guid> targetRoleIds);
    }
}
