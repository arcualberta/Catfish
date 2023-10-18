using CatfishExtensions.Interfaces.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace CatfishExtensions.Services.Auth.Requirements
{
    public class MembershipHandler : AuthorizationHandler<ICatfishTenantAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MembershipHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ICatfishTenantAuthorizationRequirement requirement)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            Guid.TryParse(request.RouteValues["tenantId"].ToString(), out Guid tenantId);
            if (tenantId == Guid.Empty)
                return Task.CompletedTask;

            if(requirement.HandleRequirementAsync(tenantId, context))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
