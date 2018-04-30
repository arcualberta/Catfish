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
        private CFUserListService mUserListService;
        private CFUserListService userListService { get { if (mUserListService == null) mUserListService = new CFUserListService(db); return mUserListService; } }

        protected abstract int GetDefaultPermissions();
        protected abstract bool IsAdmin(string userGuid);

        public SecurityServiceBase(CatfishDbContext db) : base(db)
        {

        }

        public AccessMode GetPermissions(string userGuid, Aggregation entity)
        {
            if (IsAdmin(userGuid))
            {
                return AccessMode.All;
            }

            AccessMode modes = AccessMode.None;
            List<string> userGroups = new List<string>();//TODO: get the full list of users group guids
            IList<Aggregation> visitedNodes = new List<Aggregation>();
            IList<string> visitedGuids = new List<string>();



            return modes;
        }
    }
}
