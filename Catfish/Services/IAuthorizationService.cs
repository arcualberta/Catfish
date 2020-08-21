using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface IAuthorizationService
    {
        bool IsAuthorize();
        List<string> GetAccessibleActions();

        void EnsureUserRoles(List<string> roles);
    }
}
