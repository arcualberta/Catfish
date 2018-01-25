
using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class EntityGroupService :EntityService
    {
        public EntityGroupService(CatfishDbContext db) : base(db) { }

        public IQueryable GetEntityGroups()
        {
            return Db.EntityGroups.Include(eg=>eg.EntityGroupUsers);
        }

        public EntityGroup GetEntityGroup(string id)
        {
            EntityGroup entityGroup = new EntityGroup();

            if(!string.IsNullOrEmpty(id))
            {
                Guid gId = Guid.Parse(id);
                entityGroup = Db.EntityGroups.Where(eg => eg.Id == gId).Include(eg=>eg.EntityGroupUsers)
                                 .FirstOrDefault();
            }

            return entityGroup;
        }
        public EntityGroup EditEntityGroup(EntityGroup entityGroup, List<EntityGroupUser> oldEntityGrpUsers = null)
        {
           
            try
            {
                EntityGroup entGroup = GetEntityGroup(entityGroup.Id.ToString());//check if this entityGroup existing in the database
                if (entGroup != null)
                {
                    
                   
                    //update EntityGroupUser -- this will be complecated                   
                    List<EntityGroupUser> userToRemove = oldEntityGrpUsers.Where(x => !entityGroup.EntityGroupUsers.Any(y => y.UserId == x.UserId)).ToList();
                    List<EntityGroupUser> userToAdd = entityGroup.EntityGroupUsers.Where(x => !oldEntityGrpUsers.Any(y => y.UserId == x.UserId)).ToList();

                    //1. remove user from entityGRoupUser who were no longer associated with this entityGroup
                    if(userToRemove.Count > 0)
                        Db.EntityGroupUsers.RemoveRange(userToRemove);
                    
                    //2. Add new User to be associated with this entityGroup
                    if(userToAdd.Count > 0)
                        Db.EntityGroupUsers.AddRange(userToAdd);

                    //update existing entityGroup
                    Db.Entry(entityGroup).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //add new entity group

                    entityGroup = Db.EntityGroups.Add(entityGroup);
                    Db.EntityGroupUsers.AddRange(entityGroup.EntityGroupUsers);
                   
                }
               
            }catch(Exception ex)
            {
                throw ex;
            }

            return entityGroup;
        }
        
    }
}
