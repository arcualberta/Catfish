using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Authorization;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents.Workflow;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Catfish.Core.Models.Permissions;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;

namespace Catfish.Areas.Applets.Services
{
    public class ItemAppletService : IItemAppletService
    {
        private readonly ISubmissionService _submissionService;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _dotnetAuthorizationService;
        public readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        
        public ItemAppletService(AppDbContext db, ISubmissionService submissionService, Microsoft.AspNetCore.Authorization.IAuthorizationService dotnetAuthorizationService, ErrorLog errorLog)
        {
            _appDb = db;
            _submissionService = submissionService;
            _dotnetAuthorizationService = dotnetAuthorizationService;
            _errorLog = errorLog;
        }

        public Item GetItem(Guid id, ClaimsPrincipal user)
        {
            Item authorizedItem = new Item();
            Item item = _submissionService.GetSubmissionDetails(id);

            //TODO #1: If the "user" does not have permission to read the item, throw a Catfish.Core.Exceptions.AuthorizationException

            //TODO #2: Strip-off information from that should not be visible to "user" from the item.
            var task = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read });
            task.Wait();
            if (task.Result.Succeeded)
                authorizedItem =  item;
            
             return authorizedItem;
        }

        public List<UserPermissions> GetUserPermissions(Guid itemId, ClaimsPrincipal user)
        {
            try
            {
                Item item = _appDb.Items.Where(i => i.Id == itemId).FirstOrDefault();
                List<UserPermissions> userPermissions = new List<UserPermissions>();

                foreach (var form in item.DataContainer)
                    userPermissions.Add(GetUserPermissions(item, form, user));

                foreach (var form in item.MetadataSets)
                    userPermissions.Add(GetUserPermissions(item, form, user));

                return userPermissions;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public UserPermissions GetUserPermissions(Item item, FieldContainer form, ClaimsPrincipal user)
        {
            try
            {
                List<UserPermissions> userPermissions = new List<UserPermissions>();
                var myAggregatedActionList = typeof(TemplateOperations).GetFields();

                List<Permission> authorizedOperations = new List<Permission>();

                //In the current implementation of our workflow-security model, we cannot control the permissions at
                //form (or DataItem) level or metadata-set level. Therefore, if the currently logged in user is a sys admin
                //we grant all permissions to all forms. If the current user is not a sys admin, we follow the permission process
                //defined in the workflow, in which case the user may get either read or delete permissions for non-root fomrs and
                //metadata sets provided the user has CHildFormView or ChildFormDelete permissions on the item.
                if (user.IsInRole("SysAdmin"))
                {
                    authorizedOperations.Add(new Permission() { Action = TemplateOperations.Read.Name });
                    authorizedOperations.Add(new Permission() { Action = TemplateOperations.Update.Name });
                    authorizedOperations.Add(new Permission() { Action = TemplateOperations.Delete.Name });
                    authorizedOperations.Add(new Permission() { Action = TemplateOperations.ListInstances.Name });
                }
                else
                {
                    if ((form is DataItem) && (form as DataItem).IsRoot)
                    {
                        //check for all permissions other than ChildFormView and ChildFormDelete and if the user has those permissions then add them to the authorizedOperations

                        foreach (var actionItem in myAggregatedActionList)
                        {
                            var task = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { new OperationAuthorizationRequirement() { Name = actionItem.Name } });
                            task.Wait();
                            if (task.Result.Succeeded)
                            {
                                Permission actionPermission = new Permission()
                                {
                                    Action = actionItem.Name,
                                };
                                authorizedOperations.Add(actionPermission);
                            }
                        }
                    }
                    else
                    {
                        //if the user has ChildFormView permission at the item level, add "Read" permission to authorizedOperations
                        var taskRead = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { TemplateOperations.ChildFormView });
                        taskRead.Wait();
                        if (taskRead.Result.Succeeded)
                        {
                            Permission actionPermission = new Permission()
                            {
                                Action = TemplateOperations.Read.Name,
                            };
                            authorizedOperations.Add(actionPermission);
                        }

                        //if the user has ChildFormDelete permission at the item level, add "Delete" permission to authorizedOperations
                        var taskDelete = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { TemplateOperations.ChildFormDelete });
                        taskDelete.Wait();
                        if (taskDelete.Result.Succeeded)
                        {
                            Permission actionPermission = new Permission()
                            {
                                Action = TemplateOperations.Delete.Name,
                            };
                            authorizedOperations.Add(actionPermission);
                        }
                    }
                }

                UserPermissions permission = new UserPermissions()
                {
                    FormId = form.Id,
                    FormType = form.ModelType,
                    Permissions = authorizedOperations
                };

                return permission;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
    }
}
