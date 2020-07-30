using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
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

        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExist)
        {
            MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(DefaultLanguage) == metadataSetName)
                .FirstOrDefault();

            if(ms == null && createIfNotExist)
            {
                ms = new MetadataSet();
                ms.SetName(metadataSetName, DefaultLanguage);
                mEntityTemplate.MetadataSets.Add(ms);
            }
            return ms;
        }
    }
}
