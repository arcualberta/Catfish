using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public class ItemAppletService : IItemAppletService
    {
        private readonly ISubmissionService _submissionService;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _dotnetAuthorizationService;
        public readonly AppDbContext _appDb;
        public ItemAppletService(AppDbContext db, ISubmissionService submissionService, Microsoft.AspNetCore.Authorization.IAuthorizationService dotnetAuthorizationService)
        {
            _appDb = db;
            _submissionService = submissionService;
            _dotnetAuthorizationService = dotnetAuthorizationService;
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
    }
}
