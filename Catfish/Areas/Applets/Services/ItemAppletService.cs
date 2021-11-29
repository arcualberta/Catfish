using Catfish.Core.Models;
using Catfish.Services;
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
        public readonly AppDbContext _appDb;
        public ItemAppletService(AppDbContext db, ISubmissionService submissionService)
        {
            _appDb = db;
            _submissionService = submissionService;
        }

        public Item GetItem(Guid id, ClaimsPrincipal user)
        {
            Item item = _submissionService.GetSubmissionDetails(id);

            //TODO #1: If the "user" does not have permission to read the item, throw a Catfish.Core.Exceptions.AuthorizationException

            //TODO #2: Strip-off information from that should not be visible to "user" from the item.

            return item;
        }
    }
}
