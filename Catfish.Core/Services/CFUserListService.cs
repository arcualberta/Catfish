
using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class CFUserListService :EntityService
    {
        public CFUserListService(CatfishDbContext db) : base(db) { }

        public IQueryable GetEntityGroups()
        {
            return Db.CFUserLists.Include(eg=>eg.CFUserListEntries);
        }

        public CFUserList GetEntityGroup(string id)
        {
            CFUserList entityGroup = new CFUserList();

            if(!string.IsNullOrEmpty(id))
            {
                Guid gId = Guid.Parse(id);
                entityGroup = Db.CFUserLists.Where(eg => eg.Id == gId).Include(eg=>eg.CFUserListEntries)
                                 .FirstOrDefault();
            }

            return entityGroup;
        }
        public CFUserList EditEntityGroup(CFUserList entityGroup, List<CFUserListEntry> oldEntityGrpUsers = null)
        {
           
            try
            {
                CFUserList entGroup = GetEntityGroup(entityGroup.Id.ToString());//check if this entityGroup existing in the database
                if (entGroup != null)
                {
                    
                   
                    //update EntityGroupUser -- this will be complecated                   
                    List<CFUserListEntry> userToRemove = oldEntityGrpUsers.Where(x => !entityGroup.CFUserListEntries.Any(y => y.UserId == x.UserId)).ToList();
                    List<CFUserListEntry> userToAdd = entityGroup.CFUserListEntries.Where(x => !oldEntityGrpUsers.Any(y => y.UserId == x.UserId)).ToList();

                    //1. remove user from entityGRoupUser who were no longer associated with this entityGroup
                    if(userToRemove.Count > 0)
                        Db.CFUserListEntries.RemoveRange(userToRemove);
                    
                    //2. Add new User to be associated with this entityGroup
                    if(userToAdd.Count > 0)
                        Db.CFUserListEntries.AddRange(userToAdd);

                    //update existing entityGroup
                    Db.Entry(entityGroup).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //add new entity group

                    entityGroup = Db.CFUserLists.Add(entityGroup);
                    Db.CFUserListEntries.AddRange(entityGroup.CFUserListEntries);
                   
                }
               
            }catch(Exception ex)
            {
                throw ex;
            }

            return entityGroup;
        }
        
    }
}
