using Catfish.Core.Models;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public class ItemTemplateAppletService : IItemTemplateAppletService
    {
        public readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        public ItemTemplateAppletService(AppDbContext db, ErrorLog errorLog)
        {
            _appDb = db;
            _errorLog = errorLog;
        }

        public ItemTemplate GetItemTemplate(Guid id, ClaimsPrincipal user)
        {
            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == id);

            //TODO #1: If the "user" does not have permission to edit the template, throw a Catfish.Core.Exceptions.AuthorizationException

            return template;
        }
    }
}
