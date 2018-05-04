using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models.Access;
using Catfish.Core.Models;

namespace Catfish.Services
{
    public class SecurityService : SecurityServiceBase
    {
        public SecurityService(CatfishDbContext db) : base(db)
        {

        }

        protected override bool IsAdmin(string userGuid)
        {
            throw new NotImplementedException();
        }
    }
}