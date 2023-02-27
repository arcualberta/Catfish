using CatfishExtensions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces
{
    public interface IAuthApiProxy
    {
        Task<UserMembership> GetMembership(string username);
    }
}
