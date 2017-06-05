using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class EntityService: ServiceBase
    {
        public EntityService(CatfishDbContext db) : base(db) { }

        public IQueryable<EntityType> GetEntityTypes()
        {
            return Db.EntityTypes;
        }

        public EntityType GetEntityType(int id)
        {
            return Db.EntityTypes.Where(et => et.Id == id).FirstOrDefault();
        }

    }
}
