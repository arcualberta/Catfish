using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public interface IAuthorizationHelper
    {
        public IQueryable<Guid> GetGroupsAssociatedWithTemplate(Guid entityTemplateId);
        public bool HasRoleInGroup(Guid userId, List<Guid> targetGroupIds, List<Guid> targetRoleIds);
    }
}
