using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class WorkflowService : IWorkflowService
    {
        public static readonly string ReferenceNameLanguage = "en";
        
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

        public void SetEmailTemplate(
            string templateName,
            string emailSubject,
            string emailBody,
            string[] recipients,
            string contentLanguage = "en")
        {
            MetadataSet ms = GetMetadataSet(templateName);

            ms.SetFieldValue<TextField>("Subject", ReferenceNameLanguage, emailSubject, contentLanguage, true);
            ms.SetFieldValue<TextField>("Body", ReferenceNameLanguage, emailBody, contentLanguage, true);
            ms.SetFieldValue<TextField>("Recipients", ReferenceNameLanguage, recipients, contentLanguage, true);
        }

        protected MetadataSet GetMetadataSet(
            string metadataSetName,
            string metadataSetNameLanguage = "en",
            bool createIfNotExist = true)
        {
            MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(metadataSetNameLanguage) == metadataSetName)
                .FirstOrDefault();

            if(ms == null && createIfNotExist)
            {
                ms = new MetadataSet();
                ms.SetName(metadataSetName, metadataSetNameLanguage);
                mEntityTemplate.MetadataSets.Add(ms);
            }
            return ms;
        }
    }
}
