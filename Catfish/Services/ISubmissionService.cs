using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<Item> GetSubmissionList(Guid templateId, Guid collectionId, DateTime startDate, DateTime endDate);
        Item GetSubmissionDetails(Guid itemId);
        IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId);
        List<ItemField> GetAllField(string xml);
        string GetStatus(Guid? statusId);
        Item SetSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, string actionButton);
        bool SendEmail(Guid entityTemplateId);
    }
    public class ItemField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
