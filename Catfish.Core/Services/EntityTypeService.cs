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

        public CFEntityType GetEntityTypeById(int id)
        {
            return Db.EntityTypes.Include(et=>et.AttributeMappings)
                                 .Include(et=>et.MetadataSets)
                                 .Where(et => et.Id == id).FirstOrDefault();
        }
        public CFEntityType GetEntityTypeByName(string name)
        {
            return Db.EntityTypes.Where(et => et.Name == name).FirstOrDefault();
        }

        public IQueryable<CFEntityType> GetEntityTypes()
        {
            return Db.EntityTypes;
        }

        public IQueryable<CFEntityType> GetEntityTypes(CFEntityType.eTarget target)
        {
            return Db.EntityTypes.Where(et => et.TargetTypes.Contains(target.ToString())); //Mr Jan 15 2018

        }

        public bool DeleteEntityType(CFEntityType entityType)
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

        public bool UpdateEntityType(CFEntityType entityType)
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

        public IQueryable<CFEntityTypeAttributeMapping> GetEntityTypeAttributeMappings()
        {
           return Db.EntityTypeAttributeMappings.Where(a=>a.EntityTypeId != null);
        }

        public CFEntityTypeAttributeMapping GetEntityTypeAttributeMappingById(int id)
        {
            CFEntityTypeAttributeMapping entityTypeAttributeMapping =  Db.EntityTypeAttributeMappings.Where(a => a.Id == id).FirstOrDefault();
            return entityTypeAttributeMapping;
        }
    }
}
