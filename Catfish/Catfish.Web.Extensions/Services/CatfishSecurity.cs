using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Services
{
    public class CatfishSecurity : ISecurity
    {
        public Task<bool> SignIn(object context, string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task SignOut(object context)
        {
            throw new NotImplementedException();
        }
    }
}
