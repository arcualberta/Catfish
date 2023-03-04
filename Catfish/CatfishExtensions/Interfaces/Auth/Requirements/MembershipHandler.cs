using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth.Requirements
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
            var userIdInClaim = context.User.Claims.Where(claim => claim.Type == "username").FirstOrDefault();
            var request = _httpContextAccessor.HttpContext.Request;
            var tenantId = request.RouteValues["tenantId"].ToString();
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
