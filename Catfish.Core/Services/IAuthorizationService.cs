using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public interface IAuthorizationService
    {
        bool IsAuthorize();
        List<string> GetAccessibleActions();

        void EnsureGroups(List<string> workflowGroups, Guid templateId);

        void EnsureUserRoles(List<string> roles);

        /// <summary>
        /// Returns the list of entity templates that can be used by the 
        /// current user to create a new submission. This must include list of
        /// entity template sthat can be used by the public to submit new 
        /// submissions, for example in cases of public surveys.
        /// </summary>
        /// <returns></returns>
        IList<ItemTemplate> GetSubmissionTemplateList();

        /// <summary>
        /// Returns the entity template identified by the argument "id" provided
        /// if that templates can be used by the currently logged in user to create
        /// a new submission. If the public is allowed to create a new submission beased
        /// on this template, the template should be returned irrespective of who is logged in.
        /// If the user is not permitted, this should throw an AuthorizationFailed exception. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ItemTemplate GetSubmissionTemplate(Guid id);

        /// <summary>
        /// Returns an item specified by "id" for the given purpose.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Item GetItem(Guid item, AuthorizationPurpose purpose);

        string GetLoggedUserEmail();
        User GetLoggedUser();

        User GetUserDetails(Guid id);


        Role GetRole(string roleName, bool createIfNotExist);

        public Role CreateRole(string roleName, Guid roleId);
    }
}
