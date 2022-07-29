


namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishUserManager
    {
        public Task<User> ProcessExternalLogin(LoginResult externalLoginResult);
    }
}
