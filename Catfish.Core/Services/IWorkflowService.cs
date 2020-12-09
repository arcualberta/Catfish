using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Core.Services
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

        public string GetStatus(Guid templateId, string status, bool createIfNotExist, bool isEditable);

        public List<PostAction> GetPostActions(EntityTemplate entityTemplate, string function, string group);

        public string GetLoggedUserEmail();

        User GetLoggedUser();

        /// <summary>
        /// Returns the list of Groups where the specified user is associated with a role that has 
        /// authorization to perform the action specified by the requirement.
        /// </summary>
        /// <param name="entityTemplate"></param>
        /// <param name="user"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        List<Group> GetApplicableGroups(ClaimsPrincipal user, OperationAuthorizationRequirement requirement, EntityTemplate entityTemplate, Entity instance = null);

    }
}
