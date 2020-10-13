using Catfish.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class EntityAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, Entity>
    {
        public readonly IAuthorizationHelper _authHelper;
        public EntityAuthorizationHandler(IAuthorizationHelper authHelper)
        {
            _authHelper = authHelper;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Entity resource)
        {
            //The authorization is evaluated using the following criteria.
            //
            //  1. Find the Entity-Template of the Entity "resource"
            //  2. Find the "get-action" from the Entity Template such that its "function" attribute is set 
            //     to the function specified vt the "requirement".
            //  2. Find all "role-ref"s in the "authorizations" section of the above "get-action"
            //  3. If the current user holds at least one of those roles within a group where the
            //     entity template identified by the "resource" input-argument is associated with.


            throw new NotImplementedException();
        }
    }
}
