using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public class UserGroupService
    {
        protected AppDbContext Db;
        public UserGroupService(AppDbContext db)
        {
            Db = db;
        }

        public IList<Group> GetGroupList()
        {
            return Db.Groups.ToList();
        }
    }
}
