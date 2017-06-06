using Catfish.Core.Models;
using Catfish.Core.Models.Metadata;
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

        public void CreateEntityType(EntityType entityType)
        {
            Db.EntityTypes.Add(entityType);
            foreach (var m in entityType.MetadataSets)
                Db.MetadataSets.Attach(m);
        }
        public void UpdateEntityType(EntityType entityType)
        {
            Db.EntityTypes.Attach(entityType);
            foreach (var m in entityType.MetadataSets)
                Db.MetadataSets.Attach(m);
        }

    }
}
