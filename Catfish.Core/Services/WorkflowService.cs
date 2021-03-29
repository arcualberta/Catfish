﻿using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using ElmahCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Piranha;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.AspNetCore.Services;
using Piranha.Models;
using Piranha.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public class WorkflowService : IWorkflowService
    {
        public static readonly string DefaultLanguage = "en";
        private readonly ErrorLog _errorLog;
        private AppDbContext _db { get; set; }
        public readonly IdentitySQLServerDb _piranhaDb;
        private IApi _api;

        private EntityTemplate mEntityTemplate;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private Item mItem;
        private readonly IAuthorizationService _auth;

        public WorkflowService(AppDbContext db, IdentitySQLServerDb pdb, IApi api, IHttpContextAccessor httpContextAccessor, IAuthorizationService auth, ErrorLog errorLog)
        {
            _db = db;
            _piranhaDb = pdb;
            _api = api;
            _httpContextAccessor = httpContextAccessor;
            _errorLog = errorLog;
            _auth = auth;
        }

        public EntityTemplate GetModel()
        {
            return mEntityTemplate;
        }

        public EntityTemplate GetTemplate()
        {
            return mEntityTemplate;
        }

        public void SetModel(EntityTemplate entityTemplate)
        {
            mEntityTemplate = entityTemplate;
        }

        public void SetModel(Item item)
        {
            try
            {
                mItem = item;
                mEntityTemplate = _db.EntityTemplates.Where(et => et.Id == item.TemplateId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }

        }

        ////public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists)
        ////{
        ////    try
        ////    {
        ////        MetadataSet ms = GetMetadataSet(templateName, createIfNotExists, true);
        ////        return ms == null ? null : new EmailTemplate(ms.Data);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _errorLog.Log(new Error(ex));
        ////        return null;
        ////    }
        ////}
        
        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExists, bool markAsTemplateMetadataSetIfCreated)
        {
            try
            {
                MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(DefaultLanguage) == metadataSetName)
                .FirstOrDefault();

                if(ms == null && createIfNotExists)
                {
                    ms = new MetadataSet();
                    ms.SetName(metadataSetName, DefaultLanguage);
                    mEntityTemplate.MetadataSets.Add(ms);
                    if (markAsTemplateMetadataSetIfCreated)
                        ms.IsTemplate = true;
                }
                return ms;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        
        //public DataItem GetDataItem(string dataItemName, bool createIfNotExists)
        //{
        //    DataItem dataItem = mEntityTemplate.DataContainer
        //        .Where(di => di.GetName(DefaultLanguage) == dataItemName)
        //        .FirstOrDefault();

        //    if (dataItem == null && createIfNotExists)
        //    {
        //        dataItem = new DataItem();
        //        dataItem.SetName(dataItemName, DefaultLanguage);
        //        mEntityTemplate.DataContainer.Add(dataItem);
        //    }
        //    return dataItem;
        //}

        ////public Workflow GetWorkflow(bool createIfNotExists)
        ////{
        ////    try
        ////    {
        ////        XmlModel xml = new XmlModel(mEntityTemplate.Data);
        ////        XElement element = xml.GetElement(Workflow.TagName, createIfNotExists);
        ////        Workflow workflow = new Workflow(element);
        ////        return workflow;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _errorLog.Log(new Error(ex));
        ////        return null;
        ////    }
        ////}

        public List<string> GetEmailAddresses(EmailTrigger trigger)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method goes through all entity templates in the database and select all of the roles
        /// in these templates. The method should return a unique list of roles.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserRoles()
        {
            throw new NotImplementedException();
        }

        

        

        public SystemStatus GetStatus(Guid entityTemplateId, string status, bool createIfNotExist)
        {
            try
            {
                SystemStatus systemStatus = _db.SystemStatuses.Where(ss => ss.NormalizedStatus == status.ToUpper() && ss.EntityTemplateId == entityTemplateId).FirstOrDefault();
                if (systemStatus == null && createIfNotExist)
                {
                    systemStatus = new SystemStatus() { Status = status, NormalizedStatus = status.ToUpper(), Id = Guid.NewGuid(), EntityTemplateId = entityTemplateId };
                    _db.SystemStatuses.Add(systemStatus);
                    _db.SaveChanges();
                }
                return systemStatus;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
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

        /// <summary>
        /// This method returns list of post actions which are belongs to the given function and group.
        /// </summary>
        /// <param name="entityTemplate"></param>
        /// <param name="function"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<PostAction> GetPostActions(EntityTemplate entityTemplate, string function, string group)
        {
            try
            {
                //SetModel(entityTemplate);
                var workflow = entityTemplate.Workflow;
                if (workflow != null)
                {
                    var getAction = workflow.Actions.Where(ac => ac.Function == function && ac.Group == group).FirstOrDefault();
                    return getAction.PostActions.ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
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


        /// <summary>
        /// Returns the list of roles that are authorized to permform the action speified by the requirement.
        /// </summary>
        /// <param name="requirement"></param>
        /// <param name="entityTemplate"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public List<Guid> GetAuthorizedRoles(OperationAuthorizationRequirement requirement, EntityTemplate entityTemplate, Entity instance)
        {
            try
            {
                Workflow workflow = entityTemplate.Workflow;

                //Find the action that identified by the requirement from the entity template's workflow.
                var action = workflow.Actions
                    .Where(ac => ac.Function == requirement.Name)
                    .FirstOrDefault();

                //Find all roles that are authorized to perform this action by the workflow template.
                //We need to iterate through all states defined in the action and then get the roles 
                //in all these states.
                var authorizedRoleIds = new List<Guid>();
                foreach (var stateRef in action.States)
                {
                    State state = workflow.GetState(stateRef.RefId);
                    bool selectRoles = false;
                    if (string.IsNullOrWhiteSpace(state.Value))
                    {
                        //This is an empty state. Therefore, there is no need to check whether the
                        //given entity instance has the same state or not.
                        selectRoles = true;
                    }
                    else
                    {
                        //This is a none-empty state. Therefore, we need to check whether the
                        //given entity instance has the same state.

                        if (instance != null && instance.StatusId == state.Id)
                            selectRoles = true;
                    }

                    if (selectRoles)
                    {
                        foreach (var roleRef in stateRef.AuthorizedRoles)
                            authorizedRoleIds.Add(roleRef.RefId);
                    }
                }

                return authorizedRoleIds;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return new List<Guid>();
            }
        }

        /// <summary>
        /// Returns the list of Groups where the specified user is associated with a role that has 
        /// authorization to perform the action specified by the requirement.
        /// </summary>
        /// <param name="entityTemplate"></param>
        /// <param name="user"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public List<Group> GetApplicableGroups(ClaimsPrincipal user, OperationAuthorizationRequirement requirement, EntityTemplate entityTemplate, Entity instance = null)
        {
            try
            {
                //Select the list of roles that are authorzed to perform the action specified by the requirement
                var authorizedRoleIds = GetAuthorizedRoles(requirement, entityTemplate, instance);

                //Select the list of groups where the user possesses any of the authorized roles
                Guid userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var groupsWhereUserHoldAuthorizedRole = _db.UserGroupRoles
                    .Where(ugr => ugr.UserId == userId && authorizedRoleIds.Contains(ugr.GroupRole.RoleId))
                    .Select(ugr => ugr.GroupRole.Group)
                    .ToList();


                //////Select the subset of roles out of the authorizedRoles where the user belongs to.
                ////var authorizedRoles = _piranhaDb.Roles.Where(r => authorizedRoleIds.Contains(r.Id)).ToList();

                //////TODO: Filter the roles based on the user-group-role associations
                ////authorizedRoles = authorizedRoles.Where(r => user.IsInRole(r.Name)).ToList();
                ////var selectedAuthorizedRoleIds = authorizedRoles.Select(r => r.Id).ToList();

                //////Select the list of groups where the user possesses this role
                ////var groupsWhereUserHoldAuthorizedRole = _db.UserGroupRoles
                ////    .Where(ugr => ugr.UserId == userId && selectedAuthorizedRoleIds.Contains(ugr.GroupRole.RoleId))
                ////    .Select(ugr => ugr.GroupRole.Group)
                ////    .ToList();


                //Select the subset of groups with which the entity template is associated with.
                var idsOfGroupsContainsTemplate = _db.GroupTemplates
                    .Where(gt => gt.EntityTemplateId == entityTemplate.Id)
                    .Select(gt => gt.GroupId)
                    .ToList();

                var selectedGroups = groupsWhereUserHoldAuthorizedRole
                    .Where(gr => idsOfGroupsContainsTemplate.Contains(gr.Id))
                    .ToList();

                return selectedGroups;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return new List<Group>();
            }
        }

        public List<PostAction> GetAllChangeStatePostActions(EntityTemplate entityTemplate, Guid statusId)
        {
            try
            {
                //SetModel(entityTemplate);
                var workflow = entityTemplate.Workflow;
                List<PostAction> allPostActions = new List<PostAction>();
                if (workflow != null)
                {
                    var getActions = workflow.Actions.ToList();
                    foreach(var getAction in getActions)
                    {
                        var action = getAction.States.Where(st => st.RefId == statusId).ToList();
                        if (action != null)
                        {
                            foreach(var postAction in getAction.PostActions)
                            {
                                if (postAction.StateMappings.Where(sm => sm.Current == statusId).Any())
                                    allPostActions.Add(postAction);
                            }
                        }
                    }
                    return allPostActions;
                }
                return null;

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public Guid GetChildFormId(EntityTemplate entityTemplate, Guid postActionId)
        {
            try
            {
                //SetModel(entityTemplate);
                var workflow = entityTemplate.Workflow;
                foreach (var action in workflow.Actions)
                {
                    if (action.PostActions.Where(pa => pa.Id == postActionId).Any())
                    {
                        return action.Params.Select(p => p.TemplateId).FirstOrDefault();
                    }
                }
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return Guid.Empty;
            }
            
        }

        public PostAction GetPostActionByButtonId(EntityTemplate entityTemplate, Guid buttonId)
        {
            try
            {
                //SetModel(entityTemplate);
                var workflow = entityTemplate.Workflow;
                foreach (var action in workflow.Actions)
                {
                    foreach(var postAction in action.PostActions)
                    {
                        if(postAction.StateMappings.Where(sm => sm.Id == buttonId).Any())
                        {
                            return postAction;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public GetAction GetGetActionByPostActionID(EntityTemplate entityTemplate, Guid postActionId)
        {
            try
            {
                //SetModel(entityTemplate);
                var workflow = entityTemplate.Workflow;
                foreach (var action in workflow.Actions)
                {
                    if (action.PostActions.Where(pa => pa.Id == postActionId).Any())
                        return action;
                }
                return null;
            }
            catch (Exception ex)
            {

                _errorLog.Log(new Error(ex));
                return null;
            } 
        }
        public List<TriggerRef> GetTriggersByPostActionID(EntityTemplate entityTemplate, Guid statusId, Guid postActionId)
        {
            try
            {
                //SetModel(entityTemplate);
                List<TriggerRef> triggerRefs = new List<TriggerRef>();
                var action = GetGetActionByPostActionID(entityTemplate, postActionId);
                foreach (var postAction in action.PostActions)
                {
                    if (postAction.Id.Equals(postActionId))
                    {
                        var triggers = postAction.TriggerRefs.ToList();
                        foreach(var trigger in triggers)
                        {
                            if (trigger.Condition) 
                            {
                                if (trigger.NextStatus == statusId)
                                    triggerRefs.Add(trigger);
                            }
                            else
                            {
                                triggerRefs.Add(trigger);
                            }
                        }
                    }
                        //return postAction.TriggerRefs.OrderBy(tr => tr.Order).ToList();
                }
                return triggerRefs;
            }
            catch (Exception ex)
            {

                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        public Mapping GetStateMappingByStateMappingId(EntityTemplate entityTemplate, Guid stateMappingId)
        {
            try
            {
                var workflow = entityTemplate.Workflow;
                foreach(var action in workflow.Actions)
                {
                    foreach(var postAction in action.PostActions)
                    {
                        if (postAction.StateMappings.Where(sm => sm.Id == stateMappingId).Any())
                        {
                            return postAction.StateMappings.Where(sm => sm.Id == stateMappingId).FirstOrDefault();
                        }
                    }  
                }
                return null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        public ItemTemplate CreateBasicSubmissionTemplate(string templateName, string submissionFormName, string lang)
        {
            ItemTemplate template = new ItemTemplate();

            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            Workflow workflow = template.Workflow;

            //Defininig states
            State emptyState = workflow.AddState(GetStatus(template.Id, "", true));
            State submittedState = workflow.AddState(GetStatus(template.Id, "Submitted", true));
            State deleteState = workflow.AddState(GetStatus(template.Id, "Deleted", true));

            //Defininig the entry for the root data item
            DataItem inspectionForm = template.GetDataItem(submissionFormName, true, lang);
            inspectionForm.IsRoot = true;

            //Defininig roles
            WorkflowRole adminRole = workflow.AddRole(_auth.GetRole("Admin", true));
            WorkflowRole creatorRole = workflow.AddRole(_auth.GetRole("Creator", true));

            
            // ================================================
            // Create submission-instances related workflow items
            // ================================================

            GetAction startSubmissionAction = workflow.AddAction("Start Submission", nameof(TemplateOperations.Instantiate), "Home");
            startSubmissionAction.Access = GetAction.eAccess.Restricted;
            
            //Post action for submitting the form
            PostAction submitPostAction = startSubmissionAction.AddPostAction("Submit",
                                                                                nameof(TemplateOperations.Update),
                                                                                @"<p>Your Conference Fund application saved successfully. 
                                                                                You can view/edit by <a href='@SiteUrl/items/@Item.Id'>click on here</a></p>");

            submitPostAction.AddStateMapping(emptyState.Id, submittedState.Id, "Submit");

            //Defining the pop-up for the above submitPostAction action
            PopUp submitActionPopUp = submitPostAction.AddPopUp("WARNING: Submitting the Form", "Once submitted, you cannot update the form.", "");
            submitActionPopUp.AddButtons("Yes, submit", "true");
            submitActionPopUp.AddButtons("Cancel", "false");

            startSubmissionAction.AddStateReferances(emptyState.Id)
                .AddAuthorizedRole(creatorRole.Id);

            // ================================================
            // List submission-instances related workflow items
            // ================================================
            GetAction listSubmissionsAction = workflow.AddAction("List Submissions", nameof(TemplateOperations.ListInstances), "Home");
            listSubmissionsAction.Access = GetAction.eAccess.Restricted;
            listSubmissionsAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Read submission-instances related workflow items
            // ================================================
            GetAction viewSubmissionAction = workflow.AddAction("Details", nameof(TemplateOperations.Read), "List");
            viewSubmissionAction.Access = GetAction.eAccess.Restricted;
            viewSubmissionAction.AddStateReferances(submittedState.Id)
                .AddOwnerAuthorization()
                .AddAuthorizedRole(adminRole.Id);

            // ================================================
            // Edit submission-instances related workflow items
            // ================================================
            //Defining actions
            GetAction editSubmissionAction = workflow.AddAction("Edit Submission", "Edit", "Details");

            //Defining post actions
            PostAction editPostActionSave = editSubmissionAction.AddPostAction("Save", "Save");
            editPostActionSave.AddStateMapping(submittedState.Id, submittedState.Id, "Save");

            //Submissions can only be edited by admins
            editSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);



            // ================================================
            // Delete submission-instances related workflow items
            // ================================================
            //Defining actions. Only admin can delete a submission
            GetAction deleteSubmissionAction = workflow.AddAction("Delete Submission", "Delete", "Details");
            deleteSubmissionAction.AddStateReferances(submittedState.Id)
                .AddAuthorizedRole(adminRole.Id);

            //Defining post actions
            PostAction deleteSubmissionPostAction = deleteSubmissionAction.AddPostAction("Delete", "Save");
            deleteSubmissionPostAction.AddStateMapping(submittedState.Id, deleteState.Id, "Delete");

            //Defining the pop-up for the above postActionSubmit action
            PopUp deleteSubmissionActionPopUpopUp = deleteSubmissionPostAction.AddPopUp("WARNING: Delete", "Deleting the submission. Please confirm.", "");
            deleteSubmissionActionPopUpopUp.AddButtons("Yes, delete", "true");
            deleteSubmissionActionPopUpopUp.AddButtons("Cancel", "false");


            return template;
        }
    }
}
