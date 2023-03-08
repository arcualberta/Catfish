using CatfishExtensions.DTO;


namespace CatfishExtensions.Interfaces.Auth
{
    public interface IUserApiProxy
    {
        Task<string> Login(string username, string password);
        Task<bool> Register(RegistrationModel model);
        Task<UserMembership> GetMembership(string username);
        Task<List<UserInfo>> GetUsers(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null);
        Task<bool> PutUser(UserInfo dto, string? jwtToken = null);
        Task<bool> DeleteUser(string userName, string? jwtToken = null);
    }
}
