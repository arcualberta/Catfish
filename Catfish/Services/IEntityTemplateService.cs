using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface IEntityTemplateService
    {
        IList<ItemTemplate> GetItemTemplates(ClaimsPrincipal user);
        EntityTemplate GetTemplate(Guid? templateId);
        Guid GetStatusId(Guid entityTemplateId, string status);
    }
}
