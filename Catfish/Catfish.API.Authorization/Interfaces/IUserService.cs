using CatfishExtensions.DTO;

namespace Catfish.API.Authorization.Interfaces
{
    public interface IUserService
    {
        UserInfo GetUserDetails(string userName);
        UserInfo GetUserById(Guid id);
       Task<List<UserInfo>> GetUsers();
    }
}
