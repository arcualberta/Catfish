namespace Catfish.API.Authorization.Interfaces
{
    public interface IGoogleIdentity
    {
        public Task<LoginResult> GetUserLoginResult(string jwt);
    }
}
