using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsAuthorize()
        {

            return true;
        }

        public List<string> GetAccessibleActions()
        {
            List<string> authorizeList = new List<string>();
            return authorizeList;
        }
    }
}
