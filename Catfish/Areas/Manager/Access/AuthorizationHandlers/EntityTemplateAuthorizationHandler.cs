using Catfish.Areas.Manager.Access.AuthorizationRequirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access.AuthorizationHandlers
{
    public class EntityTemplateAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, EntityTemplate>
    {
        public readonly IAuthorizationHelper _authHelper;
        public EntityTemplateAuthorizationHandler(IAuthorizationHelper authHelper)
        {
            _authHelper = authHelper;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, 
            EntityTemplate resource)
        {
            //The authorization is evaluated using the following criteria.
            //
            //  1. Find the "get-action" of which the function attribute is set to "Create" in the EntityTemplate
            //     represented by the input-argument "resourse"
            //  2. Find all "role-ref"s in the "authorizations" section of the above "get-action"
            //  3. If the current user holds at least one of those roles within a group where the
            //     entity template identified by the "resource" input-argument is associated with.

            if (context == null || requirement == null || resource == null )
                throw new Exception("AuthorizationHandlerContext, Requirement, and EntityTemplate cannot be null.");

            if (resource.Workflow == null)
            {
                resource.InitializeWorkflow();
                if (resource.Workflow == null)
                    return Task.CompletedTask; //Cannot proceed on without a workflow
            }

            GetAction workflowAction = resource.Workflow.Actions.Where(ac => ac.Function == requirement.Name).FirstOrDefault();
            if(workflowAction == null)
                return Task.CompletedTask; //Cannot proceed on without a get action that matches the action identified in the requirement

            //If this is a pulicly accessible action, no further checking is needed.
            if (workflowAction.Access == GetAction.eAccess.Public)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //User must be logged in for all non-public actions
            if(!context.User.Identity.IsAuthenticated)
                return Task.CompletedTask;

            //At this point, the user is authenticated. If the action just requires any logged in user, then 
            //no further checking is needed.
            if(workflowAction.Access == GetAction.eAccess.AnyLoggedIn)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //At this point, the user is authenticated and the workflow access is restricted to specific user roles.
            //Therefore, we need to check whether the user possesses one of those roles within a group where the
            //entity template is associated with.

            List<Guid> groupdsAssociatedWithTemplate = _authHelper.GetGroupsAssociatedWithTemplate(resource.Id).ToList();
            List<Guid> authorizedRoles = workflowAction.Authorizations.Select(roleRef => roleRef.RefId).ToList();
            string currentUserIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid currentUserId = Guid.Parse(currentUserIdStr);

            bool hasRoleInGroup = _authHelper.HasRoleInGroup(
                currentUserId,
                groupdsAssociatedWithTemplate,
                authorizedRoles);

            if(hasRoleInGroup)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
