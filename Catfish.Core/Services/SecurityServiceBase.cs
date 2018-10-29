using Catfish.Core.Contexts;
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
        
        public abstract bool IsAdmin(string userGuid);

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

        public List<Guid> GetUserGuids(Guid userGuid)
        {
            List<Guid> guids = new List<Guid>();

            guids.Add(userGuid);
            guids.AddRange(
                Db.UserListEntries
                .Where(x => x.UserId == userGuid)
                .Select(x => x.CFUserListId)
                );

            return guids;
        }

        public List<Guid> GetUserGuids(string guidString)
        {
            Guid userGuid = new Guid(guidString);
            return GetUserGuids(userGuid);            
        }

        protected void CreateAccessContext(string userGuidString)
        {
            Guid guid = userGuidString == "" ? new Guid() : new Guid(userGuidString);
            CreateAccessContext(guid);
        }

        protected void CreateAccessContext(Guid userGuid)
        {
            AccessContext.current = new AccessContext(userGuid, IsAdmin(userGuid.ToString()), Db);
        }
    }
}
