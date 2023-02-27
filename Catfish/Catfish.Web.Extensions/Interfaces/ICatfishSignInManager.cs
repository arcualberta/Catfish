using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishSignInManager
    {
        Task<bool> SignIn(User user, HttpContext httpContext);
        Task<bool> SignOut(HttpContext context);
        Task AuthorizeSuccessfulExternalLogin(LoginResult externalLoginResult, HttpContext httpContext);
    }
}
