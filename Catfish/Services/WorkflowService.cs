using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Models.SiteTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class WorkflowService : IWorkflowService
    {
        public static readonly string DefaultLanguage = "en";

        private AppDbContext _db { get; set; }

        private EntityTemplate mEntityTemplate;

        private Item mItem;

        public WorkflowService(AppDbContext db)
        {
            _db = db;
        }

        public EntityTemplate GetModel()
        {
            return mEntityTemplate;
        }
        
        public EntityTemplate GetTemplate()
        {
            return mEntityTemplate;
        }

        public void SetModel(EntityTemplate entityTemplate)
        {
            mEntityTemplate = entityTemplate;
        }

        public void SetModel(Item item)
        {
            mItem = item;
            mEntityTemplate = _db.EntityTemplates.Where(et => et.Id == item.TemplateId).FirstOrDefault();
        }

        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists)
        {
            MetadataSet ms = GetMetadataSet(templateName, createIfNotExists, true);
            return ms == null ? null : new EmailTemplate(ms.Data);
        }

        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExists, bool markAsTemplateMetadataSetIfCreated)
        {
            MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(DefaultLanguage) == metadataSetName)
                .FirstOrDefault();

            if(ms == null && createIfNotExists)
            {
                ms = new MetadataSet();
                ms.SetName(metadataSetName, DefaultLanguage);
                mEntityTemplate.MetadataSets.Add(ms);
                if (markAsTemplateMetadataSetIfCreated)
                    ms.IsTemplate = true;
            }
            return ms;
        }

        //public DataItem GetDataItem(string dataItemName, bool createIfNotExists)
        //{
        //    DataItem dataItem = mEntityTemplate.DataContainer
        //        .Where(di => di.GetName(DefaultLanguage) == dataItemName)
        //        .FirstOrDefault();

        //    if (dataItem == null && createIfNotExists)
        //    {
        //        dataItem = new DataItem();
        //        dataItem.SetName(dataItemName, DefaultLanguage);
        //        mEntityTemplate.DataContainer.Add(dataItem);
        //    }
        //    return dataItem;
        //}

        public Workflow GetWorkflow(bool createIfNotExists)
        {
            XmlModel xml = new XmlModel(mEntityTemplate.Data);
            XElement element = xml.GetElement(Workflow.TagName, createIfNotExists);
            Workflow workflow = new Workflow(element);
            return workflow;
        }

        public List<string> GetEmailAddresses(EmailTrigger trigger)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method goes through all entity templates in the database and select all of the roles
        /// in these templates. The method should return a unique list of roles.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserRoles()
        {
            throw new NotImplementedException();
        }

        public void InitSiteStructure(Guid siteId, string siteTypeId)
        {
            if(siteTypeId == typeof(WorkflowPortal).Name)
            {
                var site = "test";
            }
        }
    }
}
