using Catfish.Core.Exceptions;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Handlers
{
    public class ItemEditorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Item>
    {
        public readonly IAuthorizationHelper _authHelper;
        public readonly IWorkflowService _workflowService;
        public readonly AppDbContext _appDb;

        public ItemEditorAuthorizationHandler(IAuthorizationHelper authHelper, IWorkflowService workflowService, AppDbContext db)
        {
            _authHelper = authHelper;
            _workflowService = workflowService;
            _appDb = db;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            Item resource)
        {

            if (context == null || requirement == null || resource == null)
                throw new AuthorizationException("AuthorizationHandlerContext, Requirement, and Item cannot be null.");


            if (resource == null)
            {
                return Task.CompletedTask; //Cannot proceed on without a item
            }
            else
            {
                EntityTemplate entityTemplate = _workflowService.GetEntityTemplateByEntityTemplateId(resource.TemplateId.Value);
                GetAction workflowAction = entityTemplate.Workflow.Actions.Where(ac => ac.Function == requirement.Name).FirstOrDefault();
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
                if (!context.User.Identity.IsAuthenticated) 
                {
                    throw new AuthorizationException("Authorization faild, Please login to the system.");
                }
                bool isSysAdmin = context.User.IsInRole("SysAdmin");
                if (workflowAction.Access == GetAction.eAccess.AnyLoggedIn || isSysAdmin)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                if (!resource.StatusId.HasValue)
                    return Task.CompletedTask; //Cannot evaluate authorization for objects with no status value


                //Select the state reference defined in the workflow to perform the specified action on 
                // entities with the above status
                var stateReference = workflowAction.States.Where(sr => sr.RefId == resource.StatusId).FirstOrDefault();

                if (stateReference == null)
                    return Task.CompletedTask; //If no state reference is found, we cannot continue the authorization beyond this point

                //Authorization successful if the current user is the owner of the entity AND
                //the requested action is authorized to the owner
                string currentUserEmail = _workflowService.GetLoggedUserEmail();
                if (resource.UserEmail == currentUserEmail && stateReference.IsOwnerAuthorized())
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //Authorization successful if the current user belongs to an authorized domain for
                //the requested action is authorized to the owner
                if (workflowAction.IsAuthorizedByDomain(resource.StatusId.Value, currentUserEmail))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //Authorization successful if the currrent user's email is equal to an email idenfieied
                //under AuthorizedEmailFields.
                if (workflowAction.IsAuthorizedByEmailField(entityTemplate, currentUserEmail))
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

                
            }



            return Task.CompletedTask;
        }
    }
}
