
using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class EntityGroupService :EntityService
    {
        public EntityGroupService(CatfishDbContext db) : base(db) { }

        public IQueryable GetEntityGroupList()
        {
            return Db.Entities;
        }

        public EntityGroup GetEntityGroup(string id)
        {
            EntityGroup entityGroup = new EntityGroup();

            if(!string.IsNullOrEmpty(id))
            {
                entityGroup = Db.EntityGroups.Where(eg => eg.Id == Guid.Parse(id)).FirstOrDefault();
            }

            return entityGroup;
        }
        public EntityGroup EditEntityGroup(EntityGroup entityGroup)
        {
            
            if (!string.IsNullOrEmpty(entityGroup.Id.ToString()))
            {
                //edit existing entityGroup
                Db.Entry(entityGroup).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                //add new entity group
                entityGroup.Id = new Guid();
                Db.EntityGroups.Add(entityGroup);
            }

            return entityGroup;
        }
    }
}
