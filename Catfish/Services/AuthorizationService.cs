using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public readonly AppDbContext _db;
        public AuthorizationService(AppDbContext db)
        {
            _db = db;
        }

        public bool IsAuthorize()
        {

            return true;
        }

        public List<string> GetAccessibleActions()
        {
            List<string> authorizeList = new List<string>();
            return authorizeList;
        }

        /// <summary>
        /// Iterates through the given set of user roles and adds them to the system's user roles if they
        /// do not already exist in the system.
        /// </summary>
        /// <param name="roles"></param>
        public void EnsureUserRoles(List<string> workflowRoles)
        {
            List<string> databaseRoles = new List<string>();
            var oldRoles = _db.Roles.ToList();

            foreach (var role in oldRoles)
                databaseRoles.Add(role.Name);

            List<string> NewRoles = workflowRoles.Except(databaseRoles).ToList();
        }
    }
}
