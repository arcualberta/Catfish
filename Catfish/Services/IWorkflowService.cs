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
        public EntityTemplate GetModel();
        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists);
        public DataItem GetDataItem(string dataItemName, bool createIfNotExists);


    }
}
