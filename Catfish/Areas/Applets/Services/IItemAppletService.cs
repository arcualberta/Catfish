using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public interface IItemAppletService
    {
        public Item GetItem(Guid id, ClaimsPrincipal user);
        List<string> GetUserPermissions(Guid itemId, ClaimsPrincipal user);
    }
}
