using Catfish.Core.Models;
using ElmahCore;
using Piranha.AspNetCore.Identity.Data;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class CatfishInitializationService : ICatfishInitializationService
    {
        public readonly IdentitySQLServerDb _piranhaDb;
        public readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        public CatfishInitializationService(AppDbContext adb, IdentitySQLServerDb pdb, ErrorLog errorLog)
        {
            _appDb = adb;
            _piranhaDb = pdb;
            _errorLog = errorLog;
        }

        /// <summary>
        /// This method checks the database and makes sure that it has the roles that 
        /// are required irrespective of the workflows used on the system.
        /// </summary>
        public void EnsureSystemRoles()
        {
            try
            {
                //Ensuring that the GroupAdmin role exists
                if (!_piranhaDb.Roles.Where(r => r.Name == GroupRole.GroupAdmin).Any())
                {
                    Role role = new Role();
                    role.Id = Guid.NewGuid();
                    role.Name = GroupRole.GroupAdmin;
                    role.NormalizedName = GroupRole.GroupAdmin.ToUpper();
                    _piranhaDb.Roles.Add(role);
                    _piranhaDb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }
    }
}
