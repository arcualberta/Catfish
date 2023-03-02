using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth
{
    public interface ITenantsProxy
    {
        Task<ActionResult<IEnumerable<TenantInfo>>> GetTenants(int offset = 0, int max = int.MaxValue, string? jwtBearerToken = null);
    }
}
