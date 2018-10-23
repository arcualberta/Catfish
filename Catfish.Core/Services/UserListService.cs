
using Catfish.Core.Contexts;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

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

        private void RemoveAccessGroups(IEnumerable<CFAggregation> aggregations, string id)
        {
            foreach (CFItem aggregation in aggregations)
            {
                string accessGroupXPath = "//access-group[access-guid[text()='" + id + "']]";
                aggregation.Data.XPathSelectElement(accessGroupXPath).Remove();
                aggregation.Serialize();
            }
        }

        public void DeleteEntityGroup(string id)
        {
            //check if this entityGroup existing in the database
            CFUserList userList = GetEntityGroup(id);
            if (userList != null)
            {
                // Remove from all agregations references to userList
                Guid removedGuid = new Guid(id);
                IEnumerable<CFAggregation> items = Db.Items.FindAccessibleByGuid(removedGuid, AccessMode.Read).ToList();
                IEnumerable<CFAggregation> collections = Db.Collections.FindAccessibleByGuid(removedGuid, AccessMode.Read).ToList();                

                RemoveAccessGroups(items, id);              
                RemoveAccessGroups(collections, id);

                DeleteEntityGroup(userList);
                Db.SaveChanges();

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
