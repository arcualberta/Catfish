using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
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
    public class EntityTemplateAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Entity>
    {
        public readonly IAuthorizationHelper _authHelper;
        public readonly IWorkflowService _workflowService;
        public readonly AppDbContext _appDb;
        public EntityTemplateAuthorizationHandler(IAuthorizationHelper authHelper, IWorkflowService workflowService, AppDbContext db)
        {
            _authHelper = authHelper;
            _workflowService = workflowService;
            _appDb = db;
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
                if(entity.Template == null && entity.TemplateId.HasValue && entity.TemplateId != null)
                {
                    entity.Template = _appDb.EntityTemplates.Where(et => et.Id == entity.TemplateId).FirstOrDefault();
                }
                template = entity.Template;
            }

            if (template.Workflow == null)
            {
                return Task.CompletedTask; //Cannot proceed on without a workflow
            }

            GetAction workflowAction = template.Workflow.Actions.Where(ac => ac.Function == requirement.Name).FirstOrDefault();
            if (workflowAction == null)
            {
                //TODO: Log an error stating that the operation identified by the requirement.Name is
                //      not defined in the workflow.

                return Task.CompletedTask; //Cannot proceed on without a get action that matches the action identified in the requirement
            }

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
                //instantiated entity. 

                foreach (var stateRef in workflowAction.States)
                {
                    //If the workflow action is authorized for users belong to a certain domains, then
                    //check whether the signed-in user's email belongs to one of those domains.
                    //string currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
                    string currentUserEmail = _workflowService.GetLoggedUserEmail();
                    if (workflowAction.IsAuthorizedByDomain(stateRef.RefId, currentUserEmail))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                    //At this point, the user is authenticated and the permission-requested GetAction is restricted to 
                    //specific user roles.
                    //Therefore, we need to check whether the user possesses one of those roles within a group where the
                    //entity template is associated with.

                    //Take the list of IDs of groups where the template is associated with.
                    List<Guid> groupdsAssociatedWithTemplate = _authHelper.GetGroupsAssociatedWithTemplate(resource.Id).ToList();

                    //Take the list of IDs of the roels where the current state in this iterayion is authotized with.
                    List<Guid> authorizedRoles = stateRef.AuthorizedRoles.Select(roleRef => roleRef.RefId).ToList();

                    string currentUserIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Guid currentUserId = Guid.Parse(currentUserIdStr);

                    bool hasRoleInGroup = _authHelper.HasRoleInGroup(
                        currentUserId,
                        groupdsAssociatedWithTemplate,
                        authorizedRoles);

                    if (hasRoleInGroup)
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }

                return Task.CompletedTask;
            }
            else
            {
                //Here, we are working with an instantiated object.

                var entityStatusId = entity.StatusId;
                if (!entityStatusId.HasValue)
                    return Task.CompletedTask; //Cannot evaluate authorization for objects with no status value


                //Select the state reference defined in the workflow to perform the specified action on 
                // entities with the above status
                var stateReference = workflowAction.States.Where(sr => sr.RefId == entityStatusId).FirstOrDefault();

                if(stateReference == null)
                    return Task.CompletedTask; //If no state reference is found, we cannot continue the authorization beyond this point

                //Authorization successful if the current user is the owner of the entity AND
                //the requested action is authorized to the owner
                string currentUserEmail = _workflowService.GetLoggedUserEmail();
                if (entity.UserEmail == currentUserEmail && stateReference.IsOwnerAuthorized())
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //Authorization successful if the current user belongs to an authorized domain for
                //the requested action is authorized to the owner
                if (workflowAction.IsAuthorizedByDomain(entityStatusId.Value, currentUserEmail))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //Authorization successful if the currrent user's email is equal to an email idenfieied
                //under AuthorizedEmailFields.
                if(workflowAction.IsAuthorizedByEmailField(entity, currentUserEmail))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }



                //At this point, the user is authenticated and the permission-requested GetAction is restricted to 
                //specific user roles.
                //Therefore, we need to check whether the user possesses one of those roles within a group where the
                //entity template is associated with.

                //Take the list of IDs of groups where the template is associated with.
                List<Guid> groupdsAssociatedWithTemplate = _authHelper.GetGroupsAssociatedWithTemplate(resource.TemplateId.Value).ToList();

                //Take the list of IDs of the roels where the current state in this iterayion is authotized with.
                List<Guid> authorizedRoles = stateReference.AuthorizedRoles.Select(roleRef => roleRef.RefId).ToList();

                string currentUserIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid currentUserId = Guid.Parse(currentUserIdStr);

                bool hasRoleInGroup = _authHelper.HasRoleInGroup(
                    currentUserId,
                    groupdsAssociatedWithTemplate,
                    authorizedRoles);

                if (hasRoleInGroup)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        }

        protected Task HandleRequirementAsync_BACKUP(
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

            if (context == null || requirement == null || resource == null)
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
                return Task.CompletedTask; //Cannot proceed on without a workflow
            }

            GetAction workflowAction = template.Workflow.Actions.Where(ac => ac.Function == requirement.Name).FirstOrDefault();
            if (workflowAction == null)
                return Task.CompletedTask; //Cannot proceed on without a get action that matches the action identified in the requirement

            //If this is a pulicly accessible action, no further checking is needed.
            if (workflowAction.Access == GetAction.eAccess.Public)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //User must be logged in for all non-public actions
            if (!context.User.Identity.IsAuthenticated)
                return Task.CompletedTask;

            //At this point, the user is authenticated. If the action just requires any logged in user, then 
            //no further checking is needed.
            bool isSysAdmin = context.User.IsInRole("SysAdmin");
            if (workflowAction.Access == GetAction.eAccess.AnyLoggedIn || isSysAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (entity == null)
            {
                //The passed on resource is a template and therefore, we are actually not working with any
                //instantiated entity. 

                State emptyState = template.Workflow.States.Where(st => string.IsNullOrEmpty(st.Value)).FirstOrDefault();

                if (emptyState == null)
                    return Task.CompletedTask;

                //If the workflow action is authorized for users belong to a certain domains, then
                //check whether the signed-in user's email belongs to one of those domains.
                //string currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
                string currentUserEmail = _workflowService.GetLoggedUserEmail();
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
                if (emptyStateRefInsideAction == null)
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
