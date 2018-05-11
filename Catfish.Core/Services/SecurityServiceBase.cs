using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public abstract class SecurityServiceBase : ServiceBase
    {
        private UserListService mUserListService;
        private UserListService userListService { get { if (mUserListService == null) mUserListService = new UserListService(Db); return mUserListService; } }
        
        protected abstract bool IsAdmin(string userGuid);

        public SecurityServiceBase(CatfishDbContext db) : base(db)
        {

        }

        protected AccessMode GetDefaultPermissions()
        {
            int permissions;

            if(int.TryParse(ConfigHelper.GlobalAccessModes, out permissions)){
                return (AccessMode)permissions;
            }

            return AccessMode.None;
        }

        /// <summary>
        /// NOTE: This only works for aggrigations at the moment. Future work will be needed for files and forms.
        /// </summary>
        /// <param name="userGuid">The guid of the user to compare the entity against</param>
        /// <param name="entity">The object to validate against.</param>
        /// <returns>The access modes given to the entity.</returns>
        public AccessMode GetPermissions(string userGuid, CFEntity entity)
        {
            if (typeof(CFAggregation).IsAssignableFrom(entity.GetType()))
            {
                return GetAggregationPermissions(userGuid, (CFAggregation)entity);
            }

            return AccessMode.None;
        }

        protected AccessMode GetAggregationPermissions(string userGuid, CFAggregation entity)
        {
            if (IsAdmin(userGuid))
            {
                return AccessMode.All;
            }

            AccessMode modes = AccessMode.None;
            List<string> userGroups = userListService.GetEntityGroupForUser(userGuid).Select(ul => ul.Id.ToString()).ToList();
            IList<CFAggregation> visitedNodes = new List<CFAggregation>();
            Queue<CFAggregation> entityQueue = new Queue<CFAggregation>();

            IList<string> accessableGuids = new List<string>(userGroups);
            accessableGuids.Add(userGuid);

            entityQueue.Enqueue(entity);

            while(entityQueue.Count > 0)
            {
                CFAggregation currentEntity = entityQueue.Dequeue();
                visitedNodes.Add(currentEntity);

                // Check if we have any new permissions
                foreach(CFAccessGroup accessGroup in currentEntity.AccessGroups)
                {
                    foreach(Guid guid in accessGroup.AccessGuids)
                    {
                        string guidString = guid.ToString();
                        if (accessableGuids.Contains(guidString)){
                            accessableGuids.Remove(guidString);
                            modes |= accessGroup.AccessDefinition.AccessModes;
                        }
                    }
                }

                // Move up the tree
                if(!currentEntity.BlockInheritance && modes < AccessMode.All)
                {
                    if(currentEntity.ParentMembers.Count > 0)
                    {
                        foreach(CFAggregation parent in currentEntity.ParentMembers)
                        {
                            if (!visitedNodes.Contains(parent))
                            {
                                entityQueue.Enqueue(parent);
                            }
                        }
                    }
                    else
                    {
                        modes |= GetDefaultPermissions();
                    }
                }
            }

            return modes;
        }
    }
}
