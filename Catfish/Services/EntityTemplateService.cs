using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<ItemTemplate> GetItemTemplates()
        {
            //TODO: Limit the returning list of templates to the accessible, active templates for the current user
            return _db.ItemTemplates.ToList();
        }

        public EntityTemplate GetTemplate(Guid templateId)
        {
            return _db.EntityTemplates.Where(et => et.Id == templateId).FirstOrDefault();
        }


    }
}
