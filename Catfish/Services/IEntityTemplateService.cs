using Catfish.Core.Models;
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

        //MR April 28 2021
        XmlModelList<MetadataSet> GetTemplateMetadataSets(Guid? templateId);
        FieldList GetTemplateMetadataSetFields(Guid? templateId, Guid? metadatasetId);

        //Mr Sept 22 2021
        public FieldList GetTemplateDataItemFields(Guid? templateId);
        //MR - Dec 14 2021
        public IList<SystemStatus> GetSystemStatuses(Guid entityTemplateId);

        //MR: Feb 23 2022
        public FieldList GetTemplateDataItemFields(Guid? templateId, Guid? dataItemId);
    }
}
