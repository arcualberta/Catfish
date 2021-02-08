﻿using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Helper;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IWorkflowService _workflowService;
        private ICatfishAppConfiguration _config;
        private readonly AppDbContext _db;
        private readonly ErrorLog _errorLog;
        private readonly IServiceProvider _serviceProvider;
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _dotnetAuthorizationService;
        public SubmissionService(IAuthorizationService auth, IEmailService email, IEntityTemplateService entity, IWorkflowService workflow, ICatfishAppConfiguration configuration, AppDbContext db, ErrorLog errorLog, IServiceProvider serviceProvider, Microsoft.AspNetCore.Authorization.IAuthorizationService dotnetAuthorizationService)
        {
            _authorizationService = auth;
            _emailService = email;
            _entityTemplateService = entity;
            _workflowService = workflow;
            _config = configuration;
            _db = db;
            _errorLog = errorLog;
            _serviceProvider = serviceProvider;
            _dotnetAuthorizationService = dotnetAuthorizationService;
        }

        ///// <summary>
        ///// Get the list of entities created from a give template and with the 
        ///// state identified by the given stateGuid. The returned list is limited 
        ///// to the entites that are accessible to the curretly logged in user. 
        ///// </summary>
        ///// <returns></returns>
        //public List<Entity> GetEntityList(Guid? templateId, Guid? stateGuid)
        //{
        //    //var ev = _db.Entities.Where(e => e.TemplateId == templateId && StateId == stateGuid ?).FirstOrDefault();
        //    List<Entity> authorizeList = new List<Entity>();
        //    return authorizeList;
        //}

        ///// <summary>
        ///// Get the submission details which passing from the parameter.  
        ///// </summary>
        ///// <returns></returns>
        //public Entity GetSubmissionDetails(Guid id)
        //{
        //    Entity ev = _db.Entities.Where(e => e.Id == id).FirstOrDefault();
        //    return ev;
        //}

        ///// <summary>
        ///// Save submission details which passing from the parameter.   
        ///// </summary>
        ///// <returns></returns>
        //public string SaveSubmission(Entity submission)
        //{
        //    string statusMessege="Submission Saved Sucessfully";
        //    return statusMessege;
        //}
        public List<Item> GetSubmissionList()
        {
            List<Item> items = new List<Item>();
           
            try
            {
                items = _db.Items.ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return items;

        }

        public List<Item> GetSubmissionList(ClaimsPrincipal user, Guid templateId, Guid? collectionId, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<Item> items = new List<Item>();
           // Guid gTemplateId = !string.IsNullOrEmpty(templateId) ? Guid.Parse(templateId) : Guid.Empty;
          
            DateTime from = startDate == null ? DateTime.MinValue : startDate.Value;
            DateTime to = endDate == null? DateTime.Now : endDate.Value;
            try
            {
                var query = _db.Items.Where(i=>i.TemplateId == templateId && (i.Created >= from && i.Created < to));

                var tmp = query.ToList();

                if (collectionId != Guid.Empty)
                {
                   
                    query = query.Where(i => i.PrimaryCollectionId == collectionId);
                }

                var potentialItems = query.OrderByDescending(i => i.Created).ToList();
                foreach (var item in potentialItems)
                {
                    //The user needs Read permission to view this item both in the item-details form and as a list entry
                    var task = _dotnetAuthorizationService.AuthorizeAsync(user, item, new List<IAuthorizationRequirement>() { TemplateOperations.Read });
                    task.Wait();

                    if (task.Result.Succeeded)
                        items.Add(item);
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return items;

        }

        ///// <summary>
        ///// Get the submission details which passing from the parameter.  
        ///// </summary>
        ///// <returns></returns>
        public Item GetSubmissionDetails(Guid itemId)
        {
            Item itemDetails = new Item();
            try
            {
                itemDetails = _db.Items.Where(it => it.Id == itemId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return itemDetails;
        }

        /////// <summary>
        /////// Get the submission list which passing the parameters
        /////// if the collection id is null then it just return items which are belongs to the template id
        /////// otherwise that should return items which are belongs to the template id and collection id
        /////// </summary>
        /////// <param name="templateId"></param>
        /////// <param name="collectionId"></param>
        /////// <returns></returns>
        ////public IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId)
        ////{
        ////    IList<Item> itemList = new List<Item>();
        ////    try
        ////    {
        ////        var query = _db.Items
        ////            .Include(i => i.Status)
        ////            .Where(i => i.TemplateId == templateId);

        ////        if (collectionId != null)
        ////            query = query.Where(i => i.PrimaryCollectionId == collectionId);

        ////        query = query.OrderBy(i => i.Created);

        ////        itemList = query.ToList();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _errorLog.Log(new Error(ex));
        ////    }
        ////    return itemList;
        ////}

        public SystemStatus GetStatus(Guid? statusId)
        {
            return _db.SystemStatuses.Where(s => s.Id == statusId).FirstOrDefault();
        }
      
        public List<ItemField> GetAllField(string xml)
        {
            List<ItemField> lFields = new List<ItemField>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList parentNode = doc.GetElementsByTagName("fields");
            foreach (XmlNode childrenNode in parentNode)
            {
               
                foreach (XmlNode child in childrenNode.ChildNodes)//field
                {
                    ItemField item = new ItemField();
                    foreach (XmlNode c in child.ChildNodes)//field
                    {

                        if (c.Name == "name")
                        {
                            item.FieldName = c.InnerText;
                        }
                        else if (c.Name == "values")
                        {
                            item.FieldValue = c.FirstChild.InnerText;
                        }
                    }
                    lFields.Add(item);
                }
              
                
            }

            return lFields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="entityTemplateId"></param>
        /// <param name="collectionId"></param>
        /// <param name="actionButton"></param>
        /// <returns></returns>
        public Item SetSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, Guid? groupId, string status, string action, string fileNames=null)
        {
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
                if (template == null)
                    throw new Exception("Entity template with ID = " + entityTemplateId + " not found.");

                //When we instantantiate an instance from the template, we do not need to clone metadata sets
                Item newItem = template.Instantiate<Item>();
                newItem.StatusId = _entityTemplateService.GetSystemStatus(entityTemplateId, status).Id;
                newItem.PrimaryCollectionId = collectionId;
                newItem.TemplateId = entityTemplateId;
                newItem.UserEmail = _workflowService.GetLoggedUserEmail();

                DataItem newDataItem = template.InstantiateDataItem((Guid)value.TemplateId);
                newDataItem.UpdateFieldValues(value);
                newItem.DataContainer.Add(newDataItem);
                newDataItem.EntityId = newItem.Id;

                User user = _workflowService.GetLoggedUser();
                var fromState = template.Workflow.States.Where(st => st.Value == "").Select(st => st.Id).FirstOrDefault();
                newItem.AddAuditEntry(user.Id,
                    fromState,
                    newItem.StatusId.Value,
                    action
                    );

                if (groupId.HasValue)
                    newItem.GroupId = groupId;

                return newItem;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            
        }

        /// <summary>
        /// This method used to execute all triggers in a given workflow. need to pass the entity template, function and group.
        /// </summary>
        /// <param name="entityTemplateId"></param>
        /// <param name="actionButton"></param>
        /// <param name="function"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool ExecuteTriggers(Guid entityTemplateId, string actionButton, string function, string group)
        {
            try
            {
                // get entity template using entityTemplateId
                EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);

                // get list trigger referances of given template, function and group. 
                List<TriggerRef> triggerRefs = _workflowService.GetPostActions(template, function, group)
                                                .Where(pa => pa.ButtonLabel == actionButton).FirstOrDefault()
                                                .TriggerRefs.OrderBy(tr => tr.Order).ToList();
                //need to go through all trigger referances and execute them one by one.
                bool triggerExecutionStatus = true;
                foreach (var triggerRef in triggerRefs)
                {
                    Trigger selectedTrigger = template.Workflow.Triggers.Where(tr => tr.Id == triggerRef.RefId).FirstOrDefault();
                    triggerExecutionStatus &= selectedTrigger.Execute(template, triggerRef, _serviceProvider);
                }
                return triggerExecutionStatus;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        public Item StatusChange(Guid entityId, Guid currentStatusId, Guid nextStatusId, string action)
        {
            Item item = _db.Items.Where(i => i.Id == entityId).FirstOrDefault();
            item.StatusId = nextStatusId;
            item.Updated = DateTime.Now;
            User user = _workflowService.GetLoggedUser();
            item.AddAuditEntry(user.Id,
                currentStatusId,
                nextStatusId,
                action);
            return item;
        }

    }
}
