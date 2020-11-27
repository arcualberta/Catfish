using Catfish.Core.Models;
using ElmahCore;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public SystemStatus GetStatus(Guid entityTemplateId, string status, bool createIfNotExist)
        {
            try
            {
                SystemStatus sysStatus =  _db.SystemStatuses
                    .Where(ss => ss.NormalizedStatus == status.ToUpper() && ss.EntityTemplateId == entityTemplateId)
                    .FirstOrDefault();

                if(sysStatus == null  && createIfNotExist)
                {
                    sysStatus = new SystemStatus()
                    {
                        EntityTemplateId = entityTemplateId,
                        NormalizedStatus = status.ToUpper(),
                        Status = status,
                        Id = Guid.NewGuid()
                    };
                    _db.SystemStatuses.Add(sysStatus);
                }

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

    }
}
