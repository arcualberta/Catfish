using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    abstract class SecurityServiceBase : ServiceBase
    {
        private UserListService mUserListService;
        private UserListService userListService { get { if (mUserListService == null) mUserListService = new UserListService(Db); return mUserListService; } }

        protected abstract int GetDefaultPermissions();
        protected abstract bool IsAdmin(string userGuid);

        public SecurityServiceBase(CatfishDbContext db) : base(db)
        {

        }

        public AccessMode GetPermissions(string userGuid, CFAggregation entity)
        {
            if (IsAdmin(userGuid))
            {
                return AccessMode.All;
            }

            AccessMode modes = AccessMode.None;
            List<string> userGroups = new List<string>();//TODO: get the full list of users group guids
            IList<CFAggregation> visitedNodes = new List<CFAggregation>();
            IList<string> visitedGuids = new List<string>();



            return modes;
        }
    }
}
