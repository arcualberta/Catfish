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

        public List<string> GetUserPermissions(Guid itemId, ClaimsPrincipal user)
        {
            try
            {
                //List<string> userPermissions = new List<string>();
                Item item = _appDb.Items.Where(i => i.Id == itemId).FirstOrDefault();
                EntityTemplate template = _appDb.EntityTemplates.Where(et => et.Id == item.TemplateId).FirstOrDefault();
                List<GetAction> getActions = template.Workflow.Actions.ToList();
                UserPermissions userPermissions = new UserPermissions();
                //User loggedUser = GetLoggedUser();

                var myAggregatedActionList = typeof(TemplateOperations).GetFields();

                foreach (var form in item.DataContainer)
                {
                    List<string> authorizedOperations = new List<string>();

                    if (form.IsRoot)
                    {
                        //check for all permissions other than ChildFormView and ChildFormDelete and if the user has those permissions then add them to the authorizedOperations
                    }
                    else
                    {
                        //if the user has ChildFormView permission at the item level, add "Read" permission to authorizedOperations


                        //if the user has ChildFormDelete permission at the item level, add "Delete" permission to authorizedOperations
                    }

                    UserPermissions permission = new UserPermissions()
                    {
                        FormId = form.Id,
                        FormType = form.ModelType,
                        Permissions = authorizedOperations.ToArray(),
                    };

                }


                foreach (var getAction in getActions)
                {
                    if (getAction.Access == GetAction.eAccess.Restricted)
                    {
                        var dataContainerId = getAction.Params.Select(dcid => dcid.TemplateId).FirstOrDefault();
                        var task = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { new OperationAuthorizationRequirement() { Name = getAction.Function } });
                        task.Wait();
                        if (task.Result.Succeeded)
                        {
                            var form = item.DataContainer.Where(dc => dc.Id == dataContainerId).FirstOrDefault();

                            UserPermissions permission = new UserPermissions() { 
                            FormId = form.Id,
                            FormType=form.ModelType,

                            };
                            //userPermissions.Add(getAction.Function);
                        }
                    }
                    else
                    {
                        //userPermissions.Add(getAction.Function);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
