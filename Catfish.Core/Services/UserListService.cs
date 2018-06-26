
using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class UserListService : EntityService
    {
        public UserListService(CatfishDbContext db) : base(db) { }

        public IEnumerable<CFUserList> GetAllUserLists()
        {
            return Db.UserLists.Include(eg => eg.CFUserListEntries);
        }

        public IEnumerable<CFUserList> GetEntityGroupForUser(string userId)
        {
            Guid check = Guid.Parse(userId);
            var t = GetAllUserLists().ToList().Where(ul => ul.CFUserListEntries.Any(ue => ue.UserId == check));

            return t;
        }

        public Dictionary<string, string> GetDictionaryUserLists()
        {
            Dictionary<string, string> userLists = new Dictionary<string, string>();

            userLists = Db.UserLists.Select(u => new { u.Id, u.Name }).ToDictionary(u => u.Id.ToString(), u => u.Name);
           

            return userLists;
        }

        public CFUserList GetEntityGroup(string id)
        {
            CFUserList entityGroup = new CFUserList();

            if(!string.IsNullOrEmpty(id))
            {
                Guid gId = Guid.Parse(id);
                entityGroup = Db.UserLists.Where(eg => eg.Id == gId).Include(eg=>eg.CFUserListEntries)
                                 .FirstOrDefault();
            }

            return entityGroup;
        }
        public CFUserList EditEntityGroup(CFUserList entityGroup, List<CFUserListEntry> oldEntityGrpUsers = null)
        {
           
            try
            {
                //check if this entityGroup existing in the database
                CFUserList entGroup = GetEntityGroup(entityGroup.Id.ToString());
                if (entGroup != null)
                {
                    
                   
                    //update EntityGroupUser -- this will be complecated                   
                    List<CFUserListEntry> userToRemove = oldEntityGrpUsers.Where(x => !entityGroup.CFUserListEntries.Any(y => y.UserId == x.UserId)).ToList();
                    List<CFUserListEntry> userToAdd = entityGroup.CFUserListEntries.Where(x => !oldEntityGrpUsers.Any(y => y.UserId == x.UserId)).ToList();

                    //1. remove user from entityGRoupUser who were no longer associated with this entityGroup
                    if(userToRemove.Count > 0)
                        Db.UserListEntries.RemoveRange(userToRemove);
                    
                    //2. Add new User to be associated with this entityGroup
                    if(userToAdd.Count > 0)
                        Db.UserListEntries.AddRange(userToAdd);

                    //update existing entityGroup
                    Db.Entry(entityGroup).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //add new entity group

                    entityGroup = Db.UserLists.Add(entityGroup);
                    Db.UserListEntries.AddRange(entityGroup.CFUserListEntries);
                   
                }
               
            }catch(Exception ex)
            {
                throw ex;
            }

            return entityGroup;
        }

        public void DeleteEntityGroup(string id)
        {
            //check if this entityGroup existing in the database
            CFUserList userList = GetEntityGroup(id);
            if (userList != null)
            {
                DeleteEntityGroup(userList);
            }           
        }


        public void DeleteEntityGroup(CFUserList userList)
        {
            //check if this entityGroup existing in the database
            Db.UserLists.Remove(userList);
            Db.SaveChanges();
        }

    }
}
