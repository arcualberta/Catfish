using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
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
        Item SetSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, Guid? groupId, string status, string action);
 //       bool SendEmail(EmailTemplate emailTemplate, string recipient);
        bool ExecuteTriggers(Guid entityTemplateId, string actionButton, string function, string group);
        Item StatusChange(Guid entityId, Guid currentStatusId, Guid nextStatusId, string action);
    }
    public class ItemField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
