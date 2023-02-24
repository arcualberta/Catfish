using Catfish.API.Authorization.Interfaces;
using CatfishExtensions.DTO;

namespace Catfish.API.Authorization.Services
{
    public class UserService : IUserService
    {
        private readonly IdentitySQLServerDb _piranhaDb;
        public UserService(IdentitySQLServerDb piranhaDb)
        {
            _piranhaDb = piranhaDb;
        }
        public UserInfo GetUserDetails(string userName)
        {
            var info = _piranhaDb.Users.Where(u => u.UserName == userName).FirstOrDefault();
            UserInfo userInfo = new UserInfo { 
                Id = info.Id,
                UserName = info.UserName,
                Email = info.Email
            };
            return userInfo;
        }
        public async Task<List<UserInfo>> GetUsers()
        {
            var info = await _piranhaDb.Users.ToListAsync();
            List<UserInfo> users = new List<UserInfo>();
            foreach (var user in info)
            {
                users.Add(new UserInfo {
                    UserName = user.UserName,
                    Email = user.Email,
                    Id = user.Id
                });
            }
            return users;
        }
    }
}
