using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Catfish.Core.Services
{
    public class GroupService: IGroupService
    {
        private readonly AppDbContext _appDb;
        private readonly IdentitySQLServerDb _piranhaDb;
        private readonly ErrorLog _errorLog;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupService(AppDbContext db, IdentitySQLServerDb pdb, IHttpContextAccessor httpContextAccessor, ErrorLog errorLog)
        {
            _appDb = db;
            _piranhaDb = pdb;
            _errorLog = errorLog;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// This will return all groups except which groups have there status as deleted.
        /// </summary>
        /// <returns></returns>
        public IList<Group> GetGroupList()
        {
            try
            {
                return _appDb.Groups.Where(gr => gr.GroupStatus != Group.eGroupStatus.Deleted).ToList();

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// This method return all users with limited with the maximum of max parameter. 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQueryable<User> GetUsers(int offset = 0, int max = 25)
        {
            try
            {
                return _piranhaDb.Users.OrderBy(u => u.UserName).Skip(offset).Take(max);

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        private List<Collection> GetCollections(int offset = 0, int max = 25)
        {
            try
            {
                return _appDb.Collections.ToList();

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }


        /// <summary>
        /// This method return users who belongs to the given role group.
        /// </summary>
        /// <param name="groupRoleId"></param>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQueryable<User> GetUsers(Guid groupRoleId, int offset = 0, int max = 25)
        {
            try
            {
                var usersWithGivenRoleInGroup = _appDb.UserGroupRoles
                .Where(ugr => ugr.GroupRoleId == groupRoleId)
                .Select(ugr => ugr.UserId)
                .ToList();

                return _piranhaDb.Users.Where(u => usersWithGivenRoleInGroup.Contains(u.Id));
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            
        }

        /// <summary>
        /// This method returns a list of user group roles which belongs to a given group role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserGroupRole> GetGroupRoleUser(Guid? id)
        {
            try
            {
                List<UserGroupRole> Users = new List<UserGroupRole>();
                Users = _appDb.UserGroupRoles
                    .Where(ugr => ugr.GroupRoleId == id)
                    .ToList();
                foreach (var user in Users)
                    user.User = GetUserDetails(user.UserId);
                return Users;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }

        }

        public ICollection<Collection> GetCollectionDetails(Guid? groupTemplateId)
        {
            try
            {
                return _appDb.GroupTemplates.Include(gt => gt.Collections).FirstOrDefault(gt => gt.Id == groupTemplateId).Collections;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
				return new List<Collection>();
            }

        }

        /// <summary>
        /// This will returns user details which belongs to a given user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserDetails(Guid userId)
        {
            try
            {
                return _piranhaDb.Users.Where(u => u.Id == userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// This will returns group role details which belongs to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupRole GetGroupRoleDetails(Guid id)
        {
            try
            {
                return _appDb.GroupRoles.Where(gr => gr.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GroupTemplate GetGroupTemplateDetails(Guid id)
        {
            try
            {
                var groupTemplate = _appDb.GroupTemplates.Where(gt => gt.Id == id).FirstOrDefault();
                return groupTemplate;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupRoleId"></param>
        /// <returns></returns>
        public UserGroupRole AddUserGroupRole(Guid userId, Guid groupRoleId)
        {
            try
            {
                UserGroupRole ugr = _appDb.UserGroupRoles.Where(ugr => ugr.UserId == userId && ugr.GroupRoleId == groupRoleId).FirstOrDefault();

                if (ugr == null)
                {
                    ugr = new UserGroupRole()
                    {
                        Id = Guid.NewGuid(),
                        GroupRoleId = groupRoleId,
                        UserId = userId,
                    };
                    _appDb.UserGroupRoles.Add(ugr);
                }

                return ugr;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public GroupTemplate AddTemplateCollections(Guid groupTemplateId, Guid collectionId)
        {
            try
            {
                GroupTemplate groupTemplate = _appDb.GroupTemplates.Where(gt => gt.Id == groupTemplateId).FirstOrDefault();
                Collection collection = _appDb.Collections.Where(c => c.Id == collectionId).FirstOrDefault();
                groupTemplate.Collections.Add(collection);
                return groupTemplate;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Guid> GetAllCollectionIds()
        {
            try
            {
                return _appDb.Collections.Select(c => c.Id).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searching"></param>
        /// <returns></returns>
        public IList<Guid> GetAllUserIds(string searching)
        {
            try
            {
                return _piranhaDb.Users.Where(usr => usr.Email.Contains(searching) || searching == null).Select(ur => ur.Id).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<Guid> GetGroupUserIds(Guid id)
        {
            try
            {
                return _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == id).Select(ugr => ugr.UserId).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public IList<Guid> GetTemplateCollecollectionIds(Guid groupTemplateId)
        {
            try
            {
                var collections =  GetCollectionDetails(groupTemplateId);
                return collections.Select(c => c.Id).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupTemplateId"></param>
        /// <param name="searching"></param>
        /// <returns></returns>
        public List<TemplateCollectionVM> SetTemplateCollectionAttribute(Guid groupTemplateId)
        {
            try
            {
                //get all collections which has the selected template
                var allTemplateCollections = GetAllCollectionIds();

                //get collectionId's who already selected for perticular template
                var addedTemplateCollections = GetTemplateCollecollectionIds(groupTemplateId);


                //get Collections which doesn't selected to a perticular group template
                var toBeAddedTemplateCollections = allTemplateCollections.Except(addedTemplateCollections).ToList();

                //get all Collection details
                var collections = GetCollections();
                List<TemplateCollectionVM> Collections = new List<TemplateCollectionVM>();
                foreach (var newCollection in toBeAddedTemplateCollections)
                {
                    try
                    {
                        var collection = collections.Where(c => c.Id == newCollection).FirstOrDefault();
                        var templateCollectionVM = new TemplateCollectionVM()
                        {
                            CollectionId = collection.Id,
                            TemplateGroupId = groupTemplateId,
                            CollectionName =collection.ConcatenatedName,
                            Assigned = false
                        };
                        Collections.Add(templateCollectionVM);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                return Collections;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="groupRoleId"></param>
            /// <param name="searching"></param>
            /// <returns></returns>
            public List<GroupRoleUserAssignmentVM> SetUserAttribute(Guid groupRoleId, string searching)
        {
            try
            {
                //get all users who have the selected role
                var allRoleUsers = GetAllUserIds(searching);
                //get userId's who already selected for perticular user group
                var addedRoleUsers = GetGroupUserIds(groupRoleId);

                //get userId's who doesn't selected to a perticular role group
                var toBeAddedRoleUsers = allRoleUsers.Except(addedRoleUsers).ToList();

                //get all user details
                var users = GetUsers();
                List<GroupRoleUserAssignmentVM> Users = new List<GroupRoleUserAssignmentVM>();
                foreach (var newUser in toBeAddedRoleUsers)
                {
                    try
                    {
                        var user = users.Where(u => u.Id == newUser).FirstOrDefault();
                        var groupRoleUserAssignmentVM = new GroupRoleUserAssignmentVM()
                        {
                            UserId = user.Id,
                            RoleGroupId = groupRoleId,
                            UserName = user.UserName,
                            Email = user.Email,
                            Assigned = false
                        };
                        Users.Add(groupRoleUserAssignmentVM);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                return Users;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetGroupRolesDetails()
        {
            try
            {
                return _piranhaDb.Roles.Where(r => r.NormalizedName != "SYSADMIN").OrderBy(r => r.Name).ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Group GetGroupDetails(Guid id)
        {
            try
            {
                return _appDb.Groups.Where(gr => gr.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<GroupTemplateAssignmentVM> SetTemplateAttribute(Guid groupId)
        {
            try
            {
                //BEGIN: Handling entity templates
                //================================
                //Getting all templates available in the system and then creating a view model that identifies which of them
                //have been assigned to the current group
                var templates = _appDb.ItemTemplates.ToList(); //All templates in the system
                var groupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == groupId).ToList();

                List<GroupTemplateAssignmentVM> templatesList = new List<GroupTemplateAssignmentVM>();
                foreach (var template in templates)
                {
                    try
                    {
                        var groupTemplateVM = new GroupTemplateAssignmentVM()
                        {
                            TemplateId = template.Id,
                            TemplateName = template.TemplateName
                        };
                        var currentAssociation = groupTemplates.Where(gt => gt.EntityTemplateId == template.Id).FirstOrDefault();
                        groupTemplateVM.TemplateGroupId = currentAssociation == null ? null as Guid? : currentAssociation.Id;
                        groupTemplateVM.Assigned = groupTemplateVM.TemplateGroupId.HasValue;
                        groupTemplateVM.HasCollections = _appDb.GroupTemplates.Include(gt=>gt.Collections).Where(gt => gt.EntityTemplateId == template.Id).Select(gt => gt.Collections).Any();

                        templatesList.Add(groupTemplateVM);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                //END: Handling entity templates
                return templatesList;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<GroupRoleAssignmentVM> SetRoleAttribute(Guid groupId)
        {
            try
            {
                //BEGIN: Handling roles
                //================================
                //Getting all roles available in the system and then creating a view model that identifies which of them
                //have been assigned to the current group
                var roles = GetGroupRolesDetails();
                var groupRoles = _appDb.GroupRoles.Where(gr => gr.GroupId == groupId).ToList();

                List<GroupRoleAssignmentVM> roleList = new List<GroupRoleAssignmentVM>();
                foreach (var role in roles)
                {
                    try
                    {
                        var groupRoleVM = new GroupRoleAssignmentVM()
                        {
                            RoleId = role.Id,
                            RoleName = role.Name,
                        };
                        var currentAssociation = groupRoles.Where(gr => gr.RoleId == role.Id).FirstOrDefault();
                        groupRoleVM.RoleGroupId = currentAssociation == null ? null as Guid? : currentAssociation.Id;
                        groupRoleVM.Assigned = groupRoleVM.RoleGroupId.HasValue;
                        groupRoleVM.HasUsers = _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == groupRoleVM.RoleGroupId).Any();
                        roleList.Add(groupRoleVM);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                return roleList;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<UserGroupRole> SetUserAttribute(Guid groupId)
        {
            try
            {
                List<UserGroupRole> users = _appDb.UserGroupRoles
                                .Where(ugr => ugr.GroupRole.GroupId == groupId)
                                .ToList();
                foreach (var user in users)
                    user.User = GetUserDetails(user.UserId);
                return users;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        public List<TemplateCollectionVM> SetCollectionAttribute(Guid groupId)
        {
            try
            {
                var templates = _appDb.ItemTemplates.ToList();
                var collections = _appDb.Collections.ToList();
                var groupTemplates = _appDb.GroupTemplates.Include(gt=>gt.Collections).Where(gt => gt.GroupId == groupId).ToList();

                List<TemplateCollectionVM> templateCollectionList = new List<TemplateCollectionVM>();

                foreach (var template in groupTemplates) 
                {
                    try
                    {
                        var templateCollectionVM = new TemplateCollectionVM()
                        {
                            TemplateGroupId = template.Id
                        };
                        //templateCollectionVM.CollectionId = template.Collections.Select()
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }
                return templateCollectionList;

            }
            catch (Exception ex)
            {

                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Group SaveGroupRoles(Group group, List<GroupRoleAssignmentVM> roles)
        {
            try
            {
                //get group details from Groups table
                Group dbGroup = GetGroupDetails(group.Id);

                if (dbGroup == null)
                {
                    dbGroup = new Group();
                }

                dbGroup.Name = group.Name;
                dbGroup.GroupStatus = group.GroupStatus;

                if (roles.Count > 0) 
                { 
                    //get group roles details from GroupRoles table
                    List<GroupRole> dbGroupRoles = _appDb.GroupRoles.Where(r => r.GroupId == group.Id).ToList();
                    //get roles associate data from interface 
                    List<GroupRoleAssignmentVM> newList = roles;
                    List<GroupRole> selectedGroupRoles = new List<GroupRole>();
                    //get all selected roles list
                    foreach (var role in newList)
                    {
                        try
                        {
                            if (role.Assigned)
                            {
                                var newGroupRole = new GroupRole();
                                if (role.RoleGroupId == null)
                                    newGroupRole.Id = Guid.NewGuid();
                                else
                                    newGroupRole.Id = (Guid)role.RoleGroupId;

                                newGroupRole.RoleId = role.RoleId;
                                newGroupRole.Group = dbGroup;
                                newGroupRole.GroupId = dbGroup.Id;
                                selectedGroupRoles.Add(newGroupRole);
                            }
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                        }
                    
                    }
                    //get all newly added roles to a list
                    List<GroupRole> newlyAddedRoles = selectedGroupRoles.Except(dbGroupRoles, new GroupRoleComparer()).ToList();
                    //get all deleted roles to a list
                    List<GroupRole> deletedRoles = dbGroupRoles.Except(selectedGroupRoles, new GroupRoleComparer()).ToList();


                    //add newly added roles to GroupRoles table
                    if (newlyAddedRoles.Count > 0)
                        foreach (var groupRole in newlyAddedRoles)
                            _appDb.GroupRoles.Add(groupRole);
                    //remove deleted roles from GroupRoles table
                    if (deletedRoles.Count > 0)
                        foreach (var groupRole in deletedRoles)
                            _appDb.GroupRoles.Remove(groupRole);
                }
                return dbGroup;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="templates"></param>
        public void SaveGroupTemplates(Group group, List<GroupTemplateAssignmentVM> templates)
        {
            try
            {
                //get group template details from GroupTemplates table
                List<GroupTemplate> dbGroupTemplates = _appDb.GroupTemplates.Where(r => r.GroupId == group.Id).ToList();
                //get templates associate data from interface 
                List<GroupTemplateAssignmentVM> newList = templates;
                List<GroupTemplate> selectedGroupTemplates = new List<GroupTemplate>();
                //get all selected template list
                foreach (var template in newList)
                {
                    try
                    {
                        if (template.Assigned)
                        {
                            var newGroupTemplate = new GroupTemplate();

                            if (template.TemplateGroupId == null)
                                newGroupTemplate.Id = Guid.NewGuid();
                            else
                                newGroupTemplate.Id = (Guid)template.TemplateGroupId;

                            newGroupTemplate.EntityTemplateId = template.TemplateId;
                            newGroupTemplate.Group = group;
                            newGroupTemplate.GroupId = group.Id;
                            selectedGroupTemplates.Add(newGroupTemplate);
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                    
                }
                //get all newly added templates to a list
                List<GroupTemplate> newlyAddedTemplates = selectedGroupTemplates.Except(dbGroupTemplates, new GroupTemplateComparer()).ToList();
                //get all deleted templates to a list
                List<GroupTemplate> deletedTemplates = dbGroupTemplates.Except(selectedGroupTemplates, new GroupTemplateComparer()).ToList();

                if (group == null)
                    throw new Exception("Group Details with ID = " + group.Id + " not found.");
                //add newly added templates to GroupTemplates table
                if (newlyAddedTemplates.Count > 0)
                    foreach (var groupTemplate in newlyAddedTemplates)
                        _appDb.GroupTemplates.Add(groupTemplate);
                //remove deleted roles from GroupTemplates table
                if (deletedTemplates.Count > 0)
                    foreach (var groupTemplate in deletedTemplates)
                        _appDb.GroupTemplates.Remove(groupTemplate);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        public void DeleteGroup(Guid groupId)
        {
            try
            {
                Group group = GetGroupDetails(groupId);
                group.GroupStatus = Group.eGroupStatus.Deleted;
                _appDb.Groups.Update(group);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool CheckUserGroupRole(Guid groupId)
        {
            try
            {
                var groupRoles = _appDb.GroupRoles.Where(gr => gr.GroupId == groupId);
                if (groupRoles == null)
                    return false;
                else
                {
                    foreach (var groupRole in groupRoles)
                    {
                        try
                        {
                            var userGroupRole = _appDb.UserGroupRoles.Where(ugr => ugr.GroupRoleId == groupRole.Id);
                            if (userGroupRole.Any())
                                return true;
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                            return false;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGroupRoleId"></param>
        public void DeleteUserGroupRole(Guid userGroupRoleId)
        {
            try
            {
                UserGroupRole userGroupRole = _appDb.UserGroupRoles.Where(ugr => ugr.Id == userGroupRoleId).FirstOrDefault();
                var groupRole = _appDb.GroupRoles.Where(gr => gr.Id == userGroupRole.GroupRoleId).FirstOrDefault();

                var roleDetails = _piranhaDb.UserRoles.Where(ur => ur.RoleId == groupRole.RoleId && ur.UserId == userGroupRole.UserId).FirstOrDefault();
                var groupRoles = _appDb.UserGroupRoles.Where(ugr => ugr.UserId == userGroupRole.UserId).Select(ugr => ugr.GroupRole);
                int countExsitingRoles = 0;
                foreach (var groupRoleData in groupRoles)
                    if (groupRoleData.RoleId.Equals(groupRole.RoleId))
                        countExsitingRoles++;
                if (userGroupRole != null)
                    _appDb.UserGroupRoles.Remove(userGroupRole);
                if (!(countExsitingRoles > 1))
                    _piranhaDb.UserRoles.Remove(roleDetails);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        public bool CheckLoggedUser(Guid userId, Guid groupRoleId)
        {
            try
            {
                string loggedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                UserGroupRole usergroupRole = _appDb.UserGroupRoles
                                            .Include(gr => gr.GroupRole)
                                            .Where(gr => gr.GroupRoleId == groupRoleId && gr.UserId == userId).FirstOrDefault();
                
                if (_httpContextAccessor.HttpContext.User.IsInRole("SysAdmin"))
                    return false;
                else if (_piranhaDb.Roles.Where(r => r.Id == usergroupRole.GroupRole.RoleId && r.NormalizedName == "GROUPADMIN").Any() && usergroupRole.UserId == Guid.Parse(loggedUserId))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        public List<string> GetUserEmailListByRole(Guid roleId, Guid groupId)
        {
            try
            {
                List<string> emailList = new List<string>();
                var groupRoleUsers = _appDb.UserGroupRoles.Include(gr => gr.GroupRole).Where(gr => gr.GroupRole.RoleId == roleId && gr.GroupRole.GroupId == groupId);

                foreach (var groupRoleUser in groupRoleUsers)
                    emailList.Add(_piranhaDb.Users.Where(u => u.Id == groupRoleUser.UserId).Select(u => u.Email).FirstOrDefault());

                return emailList;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            
        }

        public bool isGroupAdmin(Guid userId, Guid groupId)
        {
            try
            {
                Guid groupAdminRoleId = _piranhaDb.Roles
                    .Where(r => r.NormalizedName == "GROUPADMIN")
                    .Select(r => r.Id)
                    .FirstOrDefault();

                if (groupAdminRoleId == null || groupAdminRoleId == Guid.Empty)
                    throw new Exception("Group Admin role cannot be found !");

                bool isGroupAdmin = _appDb.UserGroupRoles.Include(gr => gr.GroupRole)
                    .Where(gr => gr.GroupRole.GroupId == groupId 
                              && gr.UserId == userId 
                              && gr.GroupRole.RoleId == groupAdminRoleId)
                    .Any();

                return isGroupAdmin;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        
    }
}
