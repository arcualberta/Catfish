using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public class EntityService: ServiceBase
    {
        public EntityService(CatfishDbContext db) : base(db) { }

        public IQueryable<EntityType> GetEntityTypes()
        {
            return Db.EntityTypes;
        }

        public IQueryable<EntityType> GetEntityTypes(EntityType.eTarget target)
        {
            return Db.EntityTypes.Where(et => et.TargetType == target);
        }

        public T CreateEntity<T>(int entityTypeId) where T : Entity, new()
        {
            EntityType et = Db.EntityTypes.Where(t => t.Id == entityTypeId).FirstOrDefault();
            if (et == null)
                throw new Exception("EntityType with ID " + entityTypeId + " not found");

            T entity = new T();
            entity.EntityType = et;
            entity.EntityTypeId = et.Id;
            entity.InitMetadataSet(et.MetadataSets.ToList());
            entity.SetAttribute("entity-type", et.Name);

            //removing audit trail entry that was created when creating the metadata set originally
            foreach(MetadataSet ms in entity.MetadataSets)
            {
                XElement audit = ms.Data.Element("audit");
                if (audit != null)
                    audit.Remove();
            }

            return entity;
        }

        public void CreateEntityType(EntityType entityType)
        {
            Db.EntityTypes.Add(entityType);
            foreach (var m in entityType.MetadataSets)
            {
                if (m.Id < 1)
                    continue;

                Db.MetadataSets.Attach(m);
            }
        }
        public void UpdateEntityType(EntityType entityType)
        {
            CustomComparer<MetadataSet> compare = new CustomComparer<MetadataSet>((x, y) => x.Id == y.Id);
            EntityType dbEntity = Db.EntityTypes.Where(e => e.Id == entityType.Id).FirstOrDefault();
            dbEntity.Name = entityType.Name;
            dbEntity.Description = entityType.Description;

            var deletedMetaData = dbEntity.MetadataSets.Except(entityType.MetadataSets, compare).ToList();
            deletedMetaData.ForEach(md => dbEntity.MetadataSets.Remove(md));

            var addedMetaData = entityType.MetadataSets.Except(dbEntity.MetadataSets, compare).ToList();
            foreach(MetadataSet md in addedMetaData)
            {
                if (md.Id < 1)
                    continue;

                var mdDb = Db.MetadataSets.Attach(md);
                dbEntity.MetadataSets.Add(mdDb);
            }

            Db.Entry(dbEntity).State = System.Data.Entity.EntityState.Modified;
        }

    }
}
