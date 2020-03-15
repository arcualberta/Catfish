using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Catfish.Core.Services
{
    public class EntityTypeService
    {
        protected AppDbContext Db;
        public EntityTypeService(AppDbContext db)
        {
            Db = db;
        }

        public IList<EntityTemplate> GetEntityTemplates()
        {
            return Db.EntityTemplates.ToList();
        }
    }
}
