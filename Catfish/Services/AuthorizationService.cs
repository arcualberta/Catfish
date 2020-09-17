using Catfish.Core.Models;
using Microsoft.AspNetCore.Http;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public readonly PiranhaDbContext _piranhaDb;
        public readonly AppDbContext _appDb;

        public readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizationService(AppDbContext adb, PiranhaDbContext pdb, IHttpContextAccessor httpContextAccessor)
        {
            _appDb = adb;
            _piranhaDb = pdb;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthorize()
        {

            return true;
        }

        public List<string> GetAccessibleActions()
        {
            List<string> authorizeList = new List<string>();
            return authorizeList;
        }

        /// <summary>
        /// Iterates through the given set of user roles and adds them to the system's user roles if they
        /// do not already exist in the system.
        /// </summary>
        /// <param name="roles"></param>
        public void EnsureUserRoles(List<string> workflowRoles)
        {
            List<string> databaseRoles = new List<string>();
            var oldRoles = _piranhaDb.Roles.ToList();

            foreach (var role in oldRoles)
                databaseRoles.Add(role.NormalizedName);

            List<string> newRoles = workflowRoles.Except(databaseRoles).ToList();

            foreach (var newRole in newRoles)
            {
                Role role = new Role();
                role.Id = Guid.NewGuid();
                role.Name = newRole;
                role.NormalizedName = newRole.ToUpper();
                _piranhaDb.Roles.Add(role);
            }

            _piranhaDb.SaveChanges();
        }

        /// <summary>
        /// Iterates through the given set of groups and adds them to the system's groups if they
        /// do not already exist in the system.
        /// </summary>
        /// <param name="groups"></param>
        public void EnsureGroups(List<string> workflowGroups, Guid templateId)
        {
            List<string> databaseGroups = new List<string>();
            var oldGroups = _appDb.Groups.ToList();

            foreach (var group in oldGroups)
                databaseGroups.Add(group.Name);

            List<string> newGroups = workflowGroups.Except(databaseGroups).ToList();

            foreach (var newGroup in newGroups)
            {
                Group group = new Group();
                group.Id = Guid.NewGuid();
                group.Name = newGroup;
                group.EntityTemplateId = templateId;
                _appDb.Groups.Add(group);
            }
        }

        public IList<ItemTemplate> GetSubmissionTemplateList()
        {  
            //get current logged user
            string loggedUserRole = GetLoggedUserRole();
            
            
            IList<ItemTemplate> itemTemplates = _appDb.ItemTemplates.ToList();

            return itemTemplates;
        }

        
        public Guid GetLoggedUserId()
        {
            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userDetails = _piranhaDb.Users.Where(ud => ud.UserName == userName).FirstOrDefault();

            return userDetails.Id;
        }

        public string GetLoggedUserRole()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        }

        

        /// <summary>
        /// Returns the entity template identified by the argument "id" provided
        /// if that templates can be used by the currently logged in user to create
        /// a new submission. If the public is allowed to create a new submission beased
        /// on this template, the template should be returned irrespective of who is logged in.
        /// If the user is not permitted, this should throw an AuthorizationFailed exception. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemTemplate GetSubmissionTemplate(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(Guid item, AuthorizationPurpose purpose)
        {
            if (typeof(View).IsAssignableFrom(purpose.GetType()))
            {
                //Validate the user against the read permission
            }
            else if (typeof(View).IsAssignableFrom(purpose.GetType()))
            {
                //Validate the user against the edit permission
            }


            throw new NotImplementedException();
        }
    }
}
