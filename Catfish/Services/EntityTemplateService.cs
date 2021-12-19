using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Workflow;
using ElmahCore;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class EntityTemplateService : IEntityTemplateService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _manager;
        private readonly ErrorLog _errorLog;
        public EntityTemplateService(AppDbContext db, UserManager<User> manager, ErrorLog errorLog)
        {
            _db = db;
            _manager = manager;
            _errorLog = errorLog;
        }

        public IList<ItemTemplate> GetItemTemplates(ClaimsPrincipal user)
        {
            try
            {
                List<Guid> selectTemplateIds = new List<Guid>();
                if (user != null)
                {
                    //Finding all groups where the user possess some role and then
                    //getting all templates associated with those groups
                    var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userIdStr))
                    {
                        var userId = Guid.Parse(userIdStr);
                        var groupIds = _db.UserGroupRoles
                            .Where(ugr => ugr.UserId == userId)
                            .Select(ugr => ugr.GroupRole.GroupId)
                            .Distinct()
                            .ToList();

                        var templateIds = _db.GroupTemplates
                            .Where(gt => groupIds.Contains(gt.GroupId))
                            .Select(gt => gt.EntityTemplateId)
                            .Distinct();
                        selectTemplateIds.AddRange(templateIds);
                    }

                }

                if (selectTemplateIds.Count > 0)
                {
                    try
                    {
                        return _db.ItemTemplates
                        .Where(t => selectTemplateIds.Contains(t.Id))
                        .ToList();
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                        return null;
                    }

                }
                else
                    return new List<ItemTemplate>();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }

        }

        public async Task<EntityTemplate> GetTemplateAsync(Guid? templateId)
        {
            try
            {
                return await _db.EntityTemplates
                    .FindAsync(templateId) //Handle the request asynchronously
                    .ConfigureAwait(false); //Request can be handled in a separate thread
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        public EntityTemplate GetTemplate(Guid? templateId)
        {
            try
            {
                return _db.EntityTemplates.Where(et => et.Id == templateId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public SystemStatus GetSystemStatus(Guid entityTemplateId, string status)
        {
            try
            {
                SystemStatus sysStatus =  _db.SystemStatuses
                    .Where(ss => ss.NormalizedStatus == status.ToUpper() && ss.EntityTemplateId == entityTemplateId)
                    .FirstOrDefault();
                return sysStatus;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public EntityTemplate GetTemplate(Guid? templateId, ClaimsPrincipal user)
        {
            try
            {
                EntityTemplate entityTemplate = null;
                if (user != null)
                {
                    if (user.IsInRole("SysAdmin"))
                    {
                        entityTemplate = _db.EntityTemplates.Where(et => et.Id == templateId).FirstOrDefault();
                    }
                    else
                    {
                        //TODO: Check whether this template is authprized to access by the domain of the currently logged-in user.


                        //Finding all groups where the user possess some role and then
                        //getting all templates associated with those groups
                        List<Guid> selectTemplateIds = new List<Guid>();

                        var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (!string.IsNullOrEmpty(userIdStr))
                        {
                            var userId = Guid.Parse(userIdStr);
                            var groupIds = _db.UserGroupRoles
                                .Where(ugr => ugr.UserId == userId)
                                .Select(ugr => ugr.GroupRole.GroupId)
                                .Distinct()
                                .ToList();
                            var templateIds = _db.GroupTemplates
                                .Where(gt => groupIds.Contains(gt.GroupId))
                                .Select(gt => gt.EntityTemplateId)
                                .Distinct();
                            selectTemplateIds.AddRange(templateIds);
                            if (selectTemplateIds.Count > 0)
                            {
                                foreach (Guid selectedId in selectTemplateIds)
                                {
                                    if (selectedId == templateId)
                                    {
                                        entityTemplate = _db.EntityTemplates.Where(et => et.Id == templateId).FirstOrDefault();
                                        break;
                                    }
                                }
                            }


                        }
                    }
                }
                return entityTemplate;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public XmlModelList<GetAction> GetTemplateActions(Guid? templateId)
        {
            EntityTemplate template = GetTemplate(templateId);

            XmlModel xml = new XmlModel(template.Data);
            XElement element = xml.GetElement(Workflow.TagName, false);// false -- don't cretae if not existed'
            Workflow workflow = new Workflow(element);
            
            return workflow.Actions;
         
        }

        public XmlModelList<MetadataSet> GetTemplateMetadataSets(Guid? templateId)
        {
            EntityTemplate template = GetTemplate(templateId);

            //XmlModel xml = new XmlModel(template.MetadataSets);
            //XElement element = xml.GetElement(Workflow.TagName, false);// false -- don't cretae if not existed'
            //Workflow workflow = new Workflow(element);

            return template.MetadataSets;

        }

        public FieldList GetTemplateMetadataSetFields(Guid? templateId, Guid? metadatasetId)
        {
            EntityTemplate template = GetTemplate(templateId);
            MetadataSet ms = template.MetadataSets.Where(m => m.Id == metadatasetId).FirstOrDefault();

            return ms.Fields;

        }
        /// <summary>
        /// MR: Sept 22 2021 - Get all the Field in the Data-Item in the Item Template
        /// </summary>
        /// <param name="templateId">Item Template Id</param>
        /// <returns></returns>
        public FieldList GetTemplateDataItemFields(Guid? templateId)
        {
            EntityTemplate template = GetTemplate(templateId);

            var dataItem = template.GetRootDataItem(false);
            
            return dataItem.Fields; 

        }
        /// <summary>
        /// Get Entity template statuses or states
        /// </summary>
        /// <param name="entityTemplateId">template Id</param>
        /// <returns></returns>
        public IList<SystemStatus> GetSystemStatuses(Guid entityTemplateId)
        {
            try
            {
                List<SystemStatus> sysStatuses = _db.SystemStatuses
                    .Where(ss => ss.EntityTemplateId == entityTemplateId).ToList();
                return sysStatuses;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

    }
}
