using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishSignInManager
    {
        public Task<bool> SignIn(User user, HttpContext httpContext);
        public Task<bool> SignOut(HttpContext context);
    }
}
