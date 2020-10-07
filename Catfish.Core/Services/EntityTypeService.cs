using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;

namespace Catfish.Core.Services
{
    public class EntityTypeService
    {
        protected AppDbContext Db;
        public EntityTypeService(AppDbContext db)
        {
            Db = db;
        }

        public IQueryable<EntityTemplate> GetEntityTemplates()
        {
            return Db.EntityTemplates.Select(et => et) ;
        }

        public List<EntityTemplateListEntry> GetEntityTemplateListEntries()
        {
            return Db.EntityTemplates
                .Select(et => new EntityTemplateListEntry(et))
                .ToList()
                .OrderBy(et => et.ModelType)
                .ToList();
        }


    }
}
