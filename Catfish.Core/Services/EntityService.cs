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
            using (CatfishDbContext newCtx = new CatfishDbContext())
            {
                EntityType dbEntity = newCtx.EntityTypes.Where(e => e.Id == entityType.Id).FirstOrDefault();
                var deletedMetaData = dbEntity.MetadataSets.Except(entityType.MetadataSets, new CustomComparer<MetadataSet>((x, y) => x.Id == y.Id)).ToList();
                deletedMetaData.ForEach(md => dbEntity.MetadataSets.Remove(md));
                newCtx.SaveChanges();
            }

            Db.Entry(entityType).State = System.Data.Entity.EntityState.Modified;
            foreach (var m in entityType.MetadataSets)
                Db.MetadataSets.Attach(m);
        }

    }
}
