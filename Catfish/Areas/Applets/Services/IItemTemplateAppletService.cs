using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public interface IItemTemplateAppletService
    {
        public ItemTemplate GetItemTemplate(Guid id, ClaimsPrincipal user);
        public List<Group> GetTemplateGroups(Guid? id);
    }
}
