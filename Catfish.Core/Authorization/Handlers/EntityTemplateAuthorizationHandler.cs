using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class EntityTemplateAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Entity>
    {
        public readonly IAuthorizationHelper _authHelper;
        public EntityTemplateAuthorizationHandler(IAuthorizationHelper authHelper)
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
            //  1. Find the "get-action" of which the function attribute is set to "Mame" attribute 
            //     of the "requirement"
            //     represented by the input-argument "resourse"
            //  2. Find all "role-ref"s in the "authorizations" section of the above "get-action"
            //  3. If the current user holds at least one of those roles within a group where the
            //     entity template identified by the "resource" input-argument is associated with.

            if (context == null || requirement == null || resource == null )
                throw new Exception("AuthorizationHandlerContext, Requirement, and EntityTemplate cannot be null.");

            EntityTemplate template = null;
            Entity entity = null;
            if (typeof(EntityTemplate).IsAssignableFrom(resource.GetType()))
                template = resource as EntityTemplate;
            else
            {
                entity = resource;
                template = resource.Template;
            }

            if (template.Workflow == null)
            {
                template.InitializeWorkflow();
                if (template.Workflow == null)
                    return Task.CompletedTask; //Cannot proceed on without a workflow
            }

            GetAction workflowAction = template.Workflow.Actions.Where(ac => ac.Function == requirement.Name).FirstOrDefault();
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
            bool isSysAdmin = context.User.IsInRole("SysAdmin");
            if(workflowAction.Access == GetAction.eAccess.AnyLoggedIn || isSysAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (entity == null)
            {
                //The passed on resource is a template and therefore, we are actually not working with any
                //instantiated entity. Therefore, the only applicable state would be the empty state

                State emptyState = template.Workflow.States.Where(st => string.IsNullOrEmpty(st.Value)).FirstOrDefault();
                if (emptyState == null)
                    return Task.CompletedTask;

                //If the workflow action is authorized for users belong to a certain domains, then
                //check whether the signed-in user's email belongs to one of those domains.
                string currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
                if (workflowAction.IsAuthorizedByDomain(emptyState.Id, currentUserEmail))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //At this point, the user is authenticated and the GetAction for which the authentication is requested
                //in the workflow is restricted to specific user roles.
                //Therefore, we need to check whether the user possesses one of those roles within a group where the
                //entity template is associated with.

                StateRef emptyStateRefInsideAction = workflowAction.States.Where(stRef => stRef.RefId == emptyState.Id).FirstOrDefault();
                if(emptyStateRefInsideAction == null)
                    return Task.CompletedTask;

                List<Guid> groupdsAssociatedWithTemplate = _authHelper.GetGroupsAssociatedWithTemplate(resource.Id).ToList();

                List<Guid> authorizedRoles = emptyStateRefInsideAction.AuthorizedRoles.Select(roleRef => roleRef.RefId).ToList();
                string currentUserIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid currentUserId = Guid.Parse(currentUserIdStr);

                bool hasRoleInGroup = _authHelper.HasRoleInGroup(
                    currentUserId,
                    groupdsAssociatedWithTemplate,
                    authorizedRoles);

                if (hasRoleInGroup)
                    context.Succeed(requirement);

                return Task.CompletedTask;
            }
            else
            {
                //Here, we are working with an instantiated object.

                ////If the workflow action is authorized for users belong to a certain domains, then
                ////check whether the signed-in user's email belongs to one of those domains.
                //string currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
                //if (workflowAction.IsAuthorizedByDomain(currentUserEmail))
                //{
                //    context.Succeed(requirement);
                //    return Task.CompletedTask;
                //}

                ////At this point, the user is authenticated and the workflow access is restricted to specific user roles.
                ////Therefore, we need to check whether the user possesses one of those roles within a group where the
                ////entity template is associated with.

                //List<Guid> groupdsAssociatedWithTemplate = _authHelper.GetGroupsAssociatedWithTemplate(resource.Id).ToList();
                //List<Guid> authorizedRoles = workflowAction.AuthorizedRoles.Select(roleRef => roleRef.RefId).ToList();
                //string currentUserIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Guid currentUserId = Guid.Parse(currentUserIdStr);

                //bool hasRoleInGroup = _authHelper.HasRoleInGroup(
                //    currentUserId,
                //    groupdsAssociatedWithTemplate,
                //    authorizedRoles);

                //if (hasRoleInGroup)
                //    context.Succeed(requirement);

                return Task.CompletedTask;
            }
        }
    }
}
