﻿using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Workflow;
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
        Task<EntityTemplate> GetTemplateAsync(Guid? templateId);
        EntityTemplate GetTemplate(Guid? templateId);
        SystemStatus GetSystemStatus(Guid entityTemplateId, string status);
        EntityTemplate GetTemplate(Guid? templateId, ClaimsPrincipal user);
        //MR Jan 4 2020
        XmlModelList<GetAction> GetTemplateActions(Guid? templateId);
    }
}
