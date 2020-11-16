using Catfish.Core.Models;
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
        public List<Item> GetSubmissionList();
        List<Item> GetSubmissionList(Guid templateId, Guid collectionId, DateTime startDate, DateTime endDate);
        Item GetSubmissionDetails(Guid itemId);
        IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId);
        public List<ItemField> GetAllField(string xml);
        public string GetStatus(Guid? statusId);
    }
    public class ItemField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
