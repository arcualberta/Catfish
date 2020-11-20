﻿using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Workflow;
using ElmahCore;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly AppDbContext _db;
        private readonly ErrorLog _errorLog;
        public SubmissionService(IAuthorizationService auth, IEmailService email, IEntityTemplateService entity, IWorkflowService workflow, AppDbContext db, ErrorLog errorLog)
        {
            _authorizationService = auth;
            _emailService = email;
            _entityTemplateService = entity;
            _workflowService = workflow;
            _db = db;
            _errorLog = errorLog;
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

        public List<Item> GetSubmissionList(Guid templateId, Guid collectionId, DateTime startDate, DateTime endDate)
        {
            List<Item> items = new List<Item>();
           // Guid gTemplateId = !string.IsNullOrEmpty(templateId) ? Guid.Parse(templateId) : Guid.Empty;
          
            DateTime from = startDate ==null ? DateTime.MinValue : startDate;
            DateTime to = endDate == null? DateTime.Now : endDate;
            try
            {
                var query = _db.Items.Where(i=>i.TemplateId == templateId && (i.Created >= from && i.Created < to));

                if (collectionId != Guid.Empty)
                {
                   
                    query = query.Where(i => i.PrimaryCollectionId == collectionId);
                }
                items = query.ToList();
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

        /// <summary>
        /// Get the submission list which passing the parameters
        /// if the collection id is null then it just return items which are belongs to the template id
        /// otherwise that should return items which are belongs to the template id and collection id
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId)
        {
            IList<Item> itemList = new List<Item>();
            try
            {
                var query = _db.Items.Include(i=>i.Status).Where(i => i.TemplateId == templateId);

                if (collectionId != null)
                    query = query.Where(i => i.PrimaryCollectionId == collectionId);

                itemList = query.ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return itemList;
        }

        public string GetStatus(Guid? statusId)
        {
            return _db.SystemStatuses.Where(s => s.Id == statusId).FirstOrDefault().NormalizedStatus;
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
        public Item SetSubmission(DataItem value, Guid entityTemplateId, Guid collectionId, string actionButton)
        {
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
                if (template == null)
                    throw new Exception("Entity template with ID = " + entityTemplateId + " not found.");

                //When we instantantiate an instance from the template, we do not need to clone metadata sets
                Item newItem = template.Instantiate<Item>();
                newItem.StatusId = _entityTemplateService.GetStatus(entityTemplateId, actionButton, true).Id;
                newItem.PrimaryCollectionId = collectionId;
                newItem.TemplateId = entityTemplateId;

                DataItem newDataItem = template.InstantiateDataItem((Guid)value.TemplateId);
                newDataItem.UpdateFieldValues(value);
                newItem.DataContainer.Add(newDataItem);
                newDataItem.EntityId = newItem.Id;

                return newItem;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
            
        }

        public bool SendEmail(Guid entityTemplateId)
        {
            try
            {
                EntityTemplate template = _entityTemplateService.GetTemplate(entityTemplateId);
                EmailTemplate emailTemplate = _workflowService.GetEmailTemplate(template.TemplateName, false);

                Email email = new Email();
                email.UserName = _authorizationService.GetLoggedUserEmail();
                email.Subject = emailTemplate.SubjectField;
                email.FromEmail = "iwickram@ualberta.ca";
                email.RecipientEmail = emailTemplate.RecipientsField;
                email.Body = emailTemplate.BodyField;
                _emailService.SendEmail(email);
                return true;

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
            
        }
    }
}
