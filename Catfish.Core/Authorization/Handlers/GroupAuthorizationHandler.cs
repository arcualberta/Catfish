using Catfish.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class GroupAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, Group>
    {
        public readonly IAuthorizationHelper _authHelper;
        public GroupAuthorizationHandler(IAuthorizationHelper authHelper)
        {
            _authHelper = authHelper;
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
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
