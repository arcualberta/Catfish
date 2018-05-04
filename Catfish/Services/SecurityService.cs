using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models.Access;
using Catfish.Core.Models;
using System.Configuration;

namespace Catfish.Services
{
    public class SecurityService : SecurityServiceBase
    {
        private UserService mUserService;
        private UserService UserService { get { if (mUserService == null) mUserService = new UserService(); return mUserService; } }

        public SecurityService(CatfishDbContext db) : base(db)
        {
        }

        protected override bool IsAdmin(string userGuid)
        {
            Piranha.Entities.User user = UserService.GetUserById(userGuid);

            if(user != null)
            {
                string defaultMode = ConfigurationManager.AppSettings["SecurityAdminRoleName"];

                if (string.IsNullOrEmpty(defaultMode))
                {
                    defaultMode = "ADMIN_CONTENT";
                }

                return user.Group.Permissions.Where(p => p.Name == defaultMode).Any();
            }

            return false;
        }
    }
}