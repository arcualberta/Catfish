using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class EntityTypeService : EntityService
    {
        public EntityTypeService(CatfishDbContext db) : base(db) { }

        public EntityType GetEntityTypeById(int id)
        {
            return Db.EntityTypes.Where(et => et.Id == id).FirstOrDefault();
        }
        public EntityType GetEntityTypeByName(string name)
        {
            return Db.EntityTypes.Where(et => et.Name == name).FirstOrDefault();
        }

        public IQueryable<EntityType> GetEntityTypes()
        {
            return Db.EntityTypes;
        }

        public IQueryable<EntityType> GetEntityTypes(EntityType.eTarget target)
        {
            return Db.EntityTypes.Where(et => et.TargetTypes.Contains(target.ToString())); //Mr Jan 15 2018

        }

        public bool DeleteEntityType(EntityType entityType)
        {
            try
            {
                Db.Entry(entityType).State = EntityState.Deleted;
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateEntityType(EntityType entityType)
        {
            try
            {
               if(entityType.Id > 0)
                {
                    //modified
                    Db.Entry(entityType).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //add new one
                    Db.EntityTypes.Add(entityType);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<EntityTypeAttributeMapping> GetEntityTypeAttributeMappings()
        {
           return Db.EntityTypeAttributeMappings;
        }
    }
}
