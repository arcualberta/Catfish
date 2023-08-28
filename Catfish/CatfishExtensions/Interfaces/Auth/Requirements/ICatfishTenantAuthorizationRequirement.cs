using CatfishExtensions.Services.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth.Requirements
{
    public interface ICatfishTenantAuthorizationRequirement : IAuthorizationRequirement
    {
        bool HandleRequirementAsync(Guid tenantId, AuthorizationHandlerContext context);
    }
}
