
using CatfishExtensions.Interfaces.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ARC.Security.Lib.DTO;

namespace CatfishExtensions.Services.Auth.Requirements
{
    public class BelongsToTenantRequirement : ICatfishTenantAuthorizationRequirement
    {
        public BelongsToTenantRequirement()
        {
        }

        public bool HandleRequirementAsync(Guid tenantId, AuthorizationHandlerContext context)
        {

            string? membershipStr = context.User.Identities.FirstOrDefault()?.Claims
                .FirstOrDefault(x => x.Type.Equals("membership", StringComparison.OrdinalIgnoreCase))?.Value;

            if (string.IsNullOrEmpty(membershipStr))
                return false;

            var membership = JsonConvert.DeserializeObject<UserMembershipDto>(membershipStr);
            if (membership == null)
                return false;

            var tenant = membership.Tenancy?.Where(t => t.Id == tenantId).FirstOrDefault();
            if (tenant == null)
                return false;

            //Since tenant is not null at this point, the user belongs to some role in the requested
            //tenant.
            return true;
        }
    }
}
