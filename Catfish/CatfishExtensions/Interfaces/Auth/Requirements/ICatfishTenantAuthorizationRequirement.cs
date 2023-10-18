
using Microsoft.AspNetCore.Authorization;

namespace CatfishExtensions.Interfaces.Auth.Requirements
{
    public interface ICatfishTenantAuthorizationRequirement : IAuthorizationRequirement
    {
        bool HandleRequirementAsync(Guid tenantId, AuthorizationHandlerContext context);
    }
}
