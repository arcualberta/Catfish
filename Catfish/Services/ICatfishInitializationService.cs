using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface ICatfishInitializationService
    {
        public void EnsureSystemRoles();
    }
}
