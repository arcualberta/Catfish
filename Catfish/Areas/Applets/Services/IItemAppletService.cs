using Catfish.Core.Models;
using Catfish.Core.Models.Permissions;
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
        List<UserPermissions> GetUserPermissions(Guid itemId, ClaimsPrincipal user);
    }
}
