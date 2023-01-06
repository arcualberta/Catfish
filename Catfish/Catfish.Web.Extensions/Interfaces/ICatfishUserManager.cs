


namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishUserManager
    {
        public Task<User> GetUser(LoginResult externalLoginResult);
        public Task<IList<string>> GetGlobalRoles(User user);

        public Task<string> GetUserJwtLoginTokenAsync(string userName, bool isAuthenticated);
    }
}
