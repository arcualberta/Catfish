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

        public bool IsCurrentUserAdmin()
        {
            return IsAdmin(HttpContext.Current.User.Identity.Name);
        }

        public override bool IsAdmin(string userGuid)
        {
            Piranha.Entities.User user = UserService.GetUserById(userGuid);

            if (user != null)
            {
                string defaultMode = ConfigurationManager.AppSettings["SecurityAdminRoleName"];

                if (string.IsNullOrEmpty(defaultMode))
                {
                    defaultMode = "ADMIN_ACCESS";
                }

                var grp = UserService.GetUsersGroup(user.GroupId.Value);
                if (grp == null)
                {
                    return false;
                }
                return grp.Permissions.Where(p => p.Name == defaultMode).Any();
            }
            return false;
        }
        
        public void CreateAccessContext()
        {
            CreateAccessContext(HttpContext.Current.User.Identity.Name);
        }            
    }
}