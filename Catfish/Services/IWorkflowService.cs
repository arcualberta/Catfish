using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface IWorkflowService
    {
        public void SetModel(EntityTemplate entityTemplate);
        public void SetModel(Item item);
        public EntityTemplate GetModel();
        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists);
        //public DataItem GetDataItem(string dataItemName, bool createIfNotExists);

        public Workflow GetWorkflow(bool createIfNotExist);

        public List<string> GetEmailAddresses(EmailTrigger trigger);

        public List<string> GetUserRoles();

        public EntityTemplate GetTemplate();

        public Task InitSiteStructureAsync(Guid siteId, string siteTypeId);

        public string GetStatus(Guid templateId, string status, bool createIfNotExist);

        public List<PostAction> GetPostActions(EntityTemplate entityTemplate, string function, string group);

    }
}
