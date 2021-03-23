using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class GroupAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, Group>
    {
        public readonly IAuthorizationHelper _authHelper;
        public readonly IGroupService _groupService;
        public readonly AppDbContext _appDb;
        public GroupAuthorizationHandler(IAuthorizationHelper authHelper, IGroupService groupService)
        {
            _authHelper = authHelper;
            _groupService = groupService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Group resource)
        {
            //The authorization is evaluated using the following criteria.
            //
            //  1. Find the Entity-Template of the Entity "resource"
            //  2. Find the "get-action" from the Entity Template such that its "function" attribute is set 
            //     to the function specified vt the "requirement".
            //  2. Find all "role-ref"s in the "authorizations" section of the above "get-action"
            //  3. If the current user holds at least one of those roles within a group where the
            //     entity template identified by the "resource" input-argument is associated with.

            if (context == null || requirement == null || resource == null)
                throw new Exception("AuthorizationHandlerContext, Requirement, and EntityTemplate cannot be null.");

            if(resource == null)
            {
                return Task.CompletedTask; //Cannot proceed on without a group
            }
            else
            {
                bool isSysAdmin = context.User.IsInRole("SysAdmin");
                if (isSysAdmin)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                if(requirement.Name == GroupOperations.UpdateGroup.Name)
                {
                    bool isGroupAdmin = _groupService.isGroupAdmin(Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier).Value), resource.Id);
                    if (isGroupAdmin)
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
                return Task.CompletedTask;
            }
            
        }
    }
}
