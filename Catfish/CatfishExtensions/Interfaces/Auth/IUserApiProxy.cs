using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth
{
    public interface IUserApiProxy
    {
        Task<string> Login(string username, string password);
        Task<bool> Register(RegistrationModel model);
        Task<UserMembership> GetMembership(string username);
    }
}
