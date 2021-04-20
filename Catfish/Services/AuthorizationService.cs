using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using ElmahCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public readonly IdentitySQLServerDb _piranhaDb;
        public readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;

        public readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(AppDbContext adb, IdentitySQLServerDb pdb, IHttpContextAccessor httpContextAccessor, ErrorLog errorLog)
        {
            _appDb = adb;
            _piranhaDb = pdb;
            _httpContextAccessor = httpContextAccessor;
            _errorLog = errorLog;
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
            try
            {
                List<string> databaseRoles = new List<string>();
                var oldRoles = _piranhaDb.Roles.ToList();

                foreach (var role in oldRoles)
                    databaseRoles.Add(role.Name);

                List<string> newRoles = workflowRoles.Except(databaseRoles).ToList();

                foreach (var newRole in newRoles)
                {
                    try
                    {
                        Role role = new Role();
                        role.Id = Guid.NewGuid();
                        role.Name = newRole;
                        role.NormalizedName = newRole.ToUpper();
                        _piranhaDb.Roles.Add(role);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                    
                }

                _piranhaDb.SaveChanges();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            
        }

        public Role GetRole(string roleName, bool createIfNotExist)
        {
            try
            {
                Role role = _piranhaDb.Roles.Where(r => r.Name == roleName).FirstOrDefault();
                if (role == null && createIfNotExist)
                {
                    try
                    {
                        role = new Role() { Name = roleName, NormalizedName = roleName.ToUpper(), Id = Guid.NewGuid() };
                        _piranhaDb.Roles.Add(role);
                        _piranhaDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                return role;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public Role CreateRole(string roleName, Guid roleId)
        {
            if(_piranhaDb.Roles.Where(r => r.Id == roleId).Any())
                throw new Exception(string.Format("Error: the Role with ID {0} already exist in the system.", roleId));
           
            //Creating a new status with the given GUID
            var role = new Role()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                Id = roleId
            };
            _piranhaDb.Roles.Add(role);

            _piranhaDb.SaveChanges();
            return role;
        }

        /// <summary>
        /// Iterates through the given set of groups and adds them to the system's groups if they
        /// do not already exist in the system.
        /// </summary>
        /// <param name="groups"></param>
        public void EnsureGroups(List<string> workflowGroups, Guid templateId)
        {
            try
            {
                List<string> databaseGroups = new List<string>();
                var oldGroups = _appDb.Groups.ToList();

                foreach (var group in oldGroups)
                    databaseGroups.Add(group.Name);

                List<string> newGroups = workflowGroups.Except(databaseGroups).ToList();

                foreach (var newGroup in newGroups)
                {
                    try
                    {
                        Group group = new Group();
                        group.Id = Guid.NewGuid();
                        group.Name = newGroup;
                        _appDb.Groups.Add(group);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            
        }

        public IList<ItemTemplate> GetSubmissionTemplateList()
        {
            try
            {
                string loggedUserRole = GetLoggedUserRole();


                IList<ItemTemplate> itemTemplates = _appDb.ItemTemplates.ToList();

                return itemTemplates;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            //get current logged user
            
        }
        
        public Guid GetLoggedUserId()
        {
            try
            {
                string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userDetails = _piranhaDb.Users.Where(ud => ud.UserName == userName).FirstOrDefault();

                return Guid.Parse(userName);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return new Guid();
            }
        }

        public string GetLoggedUserEmail()
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userDetails = _piranhaDb.Users.Where(ud => ud.Id == Guid.Parse(userId)).FirstOrDefault();
                return userDetails.Email;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return "";
            }
        }

        public User GetLoggedUser()
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userDetails = _piranhaDb.Users.Where(ud => ud.Id == Guid.Parse(userId)).FirstOrDefault();
                return userDetails;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public string GetLoggedUserRole()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return "";
            }
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

        
        

        public User GetUserDetails(Guid id)
        {
            try
            {
                return _piranhaDb.Users.Where(u => u.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

    }
}
