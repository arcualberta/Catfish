using Catfish.Core.Models;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class CatfishInitializationService : ICatfishInitializationService
    {
        public readonly PiranhaDbContext _piranhaDb;
        public readonly AppDbContext _appDb;
        public CatfishInitializationService(AppDbContext adb, PiranhaDbContext pdb)
        {
            _appDb = adb;
            _piranhaDb = pdb;
        }

        /// <summary>
        /// This method checks the database and makes sure that it has the roles that 
        /// are required irrespective of the workflows used on the system.
        /// </summary>
        public void EnsureSystemRoles()
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
    }
}
