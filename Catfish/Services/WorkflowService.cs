using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class WorkflowService : IWorkflowService
    {
        public static readonly string DefaultLanguage = "en";

        private EntityTemplate mEntityTemplate;

        public WorkflowService()
        {

        }

        public EntityTemplate GetModel()
        {
            return mEntityTemplate;
        }

        public void SetModel(EntityTemplate entityTemplate)
        {
            mEntityTemplate = entityTemplate;
        }

        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists)
        {
            MetadataSet ms = GetMetadataSet(templateName, createIfNotExists);
            return ms == null ? null : new EmailTemplate(ms.Data);
        }

        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExists)
        {
            MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(DefaultLanguage) == metadataSetName)
                .FirstOrDefault();

            if(ms == null && createIfNotExists)
            {
                ms = new MetadataSet();
                ms.SetName(metadataSetName, DefaultLanguage);
                mEntityTemplate.MetadataSets.Add(ms);
            }
            return ms;
        }

        public DataItem GetDataItem(string dataItemName, bool createIfNotExists)
        {
            DataItem dataItem = mEntityTemplate.DataContainer
                .Where(di => di.GetName(DefaultLanguage) == dataItemName)
                .FirstOrDefault();

            if (dataItem == null && createIfNotExists)
            {
                dataItem = new DataItem();
                dataItem.SetName(dataItemName, DefaultLanguage);
                mEntityTemplate.DataContainer.Add(dataItem);
            }
            return dataItem;
        }
    }
}
