using CatfishExtensions.DTO;

namespace Catfish.API.Auth.Interfaces
{
    public interface IAccountService
    {
        Task Seed(RegistrationModel model);
        Task CreateUser(RegistrationModel model);
        Task ChangePassword(ChangePasswordModel model, bool isAdmin);
        Task DeleteUser(string username);
        Task<string> Login(LoginModel model);
        Task<IList<UserInfo>> GetUsers(int offset = 0, int max = int.MaxValue);
    }
}
