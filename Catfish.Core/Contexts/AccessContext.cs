using Catfish.Core.Models;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Contexts
{
    class AccessContext
    {
        private CatfishDbContext mDb;
        protected CatfishDbContext Db {
            get {
                if (mDb == null)
                    mDb = new CatfishDbContext();
                return mDb;
            }
        }
      
        public Guid UserGuid { get; }
        public bool IsAdmin { get; }        
        public List<Guid> ListGuids
        {
            get
            {
                List<Guid> guids = new List<Guid>();

                guids.Add(UserGuid);
                guids.AddRange(
                    Db.UserListEntries
                    .Where(x => x.UserId == UserGuid)
                    .Select(x => x.CFUserListId)
                    );

                return guids;
            }
        }

        public List<Guid> AllGuids
        {
            get
            {
                List<Guid> guids = ListGuids;
                guids.Add(UserGuid);
                return guids;
            }
        }
        

        [ThreadStatic]
        public static AccessContext current;

        public AccessContext(Guid userGuid, bool isAdmin)
        {
            UserGuid = userGuid;
            IsAdmin = isAdmin;
        }

        protected List<Guid> GetUserGuids()
        {
            Guid userGuid = new Guid(UserGuid.ToString());
            List<Guid> guids = new List<Guid>();

            guids.Add(userGuid);
            guids.AddRange(
                Db.UserListEntries
                .Where(x => x.UserId == userGuid)
                .Select(x => x.CFUserListId)
                );

            return guids;
        }

    }
}
