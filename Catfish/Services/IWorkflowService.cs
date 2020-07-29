using Catfish.Core.Models;
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
        public void SetEmailTemplate(
            string templateName,
            string emailSubject,
            string emailBody,
            string[] recipients,
            string contentLanguage = "en");

    }
}
