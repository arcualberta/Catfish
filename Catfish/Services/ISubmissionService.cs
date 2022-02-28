using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public interface ISubmissionService
    {
        //List<Entity> GetEntityList(Guid? templateId, Guid? stateGuid);
        //Entity GetSubmissionDetails(Guid id);
        //string SaveSubmission(Entity submission);
        List<Item> GetSubmissionList();
        List<Item> GetSubmissionList(ClaimsPrincipal user, Guid templateId, Guid? collectionId, DateTime? startDate = null, DateTime? endDate = null);
        Item GetSubmissionDetails(Guid itemId);
        List<ItemField> GetAllField(string xml);
        SystemStatus GetStatus(Guid? statusId);
        Item SetSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, Guid? groupId, Guid status, string action, List<IFormFile> files = null, List<string> fileKeys = null);
        Item EditSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, Guid itemId, Guid? groupId, Guid status, string action, string fileNames = null);
        Item DeleteSubmission(Item item);
        Item AddChild(DataItem value, Guid entityTemplateId, Guid itemId, Guid stateId, Guid buttonId, string fileNames = null);
        Item DeleteChild(Guid instanceId, Guid childFormId, Guid? parentId, string fileNames = null);
        //       bool SendEmail(EmailTemplate emailTemplate, string recipient);
        bool ExecuteTriggers(Guid entityTemplateId,Item item, Guid postActionId);
        Item StatusChange(Guid entityId, Guid currentStatusId, Guid nextStatusId, string action);
        string SetSuccessMessage(Guid entityTemplateId, Guid postActionId, Guid itemId);
        List<Item> GetSubmissionList(Guid? collectionId);

        List<Collection> GetCollectionList();
    }
    public class ItemField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
