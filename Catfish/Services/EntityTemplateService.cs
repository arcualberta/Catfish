using Catfish.Core.Models;
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
        public EntityTemplateService(AppDbContext db)
        {
            _db = db;
        }

        public IList<ItemTemplate> GetItemTemplates(ClaimsPrincipal user)
        {
            List<Guid> selectTemplateIds = new List<Guid>();
            if (user != null)
            {
                //Finding all groups where the user possess some role
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
                return _db.ItemTemplates
                    .Where(t => selectTemplateIds.Contains(t.Id))
                    .ToList();
            }
            else
                return new List<ItemTemplate>();
        }

        public EntityTemplate GetTemplate(Guid templateId)
        {
            return _db.EntityTemplates.Where(et => et.Id == templateId).FirstOrDefault();
        }


    }
}
