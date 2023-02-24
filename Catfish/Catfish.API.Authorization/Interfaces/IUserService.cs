using CatfishExtensions.DTO;

namespace Catfish.API.Authorization.Interfaces
{
    public interface IUserService
    {
        UserInfo GetUserDetails(string userName);
       Task<List<UserInfo>> GetUsers();
    }
}
