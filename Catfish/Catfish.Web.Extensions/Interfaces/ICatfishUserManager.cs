


namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishUserManager
    {
        public Task<User> GetUser(LoginResult externalLoginResult);
        public Task<IList<string>> GetGlobalRoles(User user);
        public Task<bool> SignIn(User user, HttpContext httpContext);
        public Task<bool> SignOut(HttpContext context);
    }
}
