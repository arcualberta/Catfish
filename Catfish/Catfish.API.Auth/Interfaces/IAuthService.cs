using Catfish.API.Auth.Models;
using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Identity;

namespace Catfish.API.Auth.Interfaces
{
    public interface IAuthService
    {
        Task PatchTenant(AuthPatchModel model);
        Task PatchRole(AuthPatchModel model);
        Task UpdateRole(TenantRole model);
        Task<UserMembership> GetMembership(IdentityUser user);
    }
}
