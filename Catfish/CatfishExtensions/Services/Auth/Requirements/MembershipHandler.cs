using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Services.Auth.Requirements
{
    public class MembershipHandler : AuthorizationHandler<MembershipRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MembershipHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MembershipRequirement requirement)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            Guid.TryParse(request.RouteValues["tenantId"].ToString(), out Guid tenantId);
            if(tenantId == Guid.Empty)
                return Task.CompletedTask;

            string? membershipStr = context.User.Identities.FirstOrDefault()?.Claims
                .FirstOrDefault(x => x.Type.Equals("membership", StringComparison.OrdinalIgnoreCase))?.Value;

            if(string.IsNullOrEmpty(membershipStr))
                return Task.CompletedTask;

            var membership = JsonConvert.DeserializeObject<UserMembership>(membershipStr);
            if (membership == null)
                return Task.CompletedTask;

            var tenant = membership.Tenancy?.Where(t => t.Id == tenantId).FirstOrDefault();
            if(tenant == null)
                return Task.CompletedTask;

            //See whether the user is a member under any role in the required tenant

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
