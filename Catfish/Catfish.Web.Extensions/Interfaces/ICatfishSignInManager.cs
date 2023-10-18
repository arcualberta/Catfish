using ARC.Security.Lib.Google.DTO;

namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishSignInManager
    {
        Task<bool> SignOut(HttpContext context);
        Task AuthorizeSuccessfulExternalLogin(LoginResult externalLoginResult, HttpContext httpContext);
    }
}
