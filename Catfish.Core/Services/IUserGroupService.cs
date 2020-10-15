using Catfish.Core.Models;
using Piranha.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    interface IUserGroupService
    {
        IList<Group> GetGroupList();
        IQueryable<User> GetUsers(int offset = 0, int max = 25);
    }
}
