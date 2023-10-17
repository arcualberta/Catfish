using Catfish.API.Auth.Models;
using ARC.Security.Lib.DTO;
using Microsoft.AspNetCore.Identity;

namespace Catfish.API.Auth.Interfaces
{
    public interface IAuthService
    {
        Task PatchTenant(AuthPatchModel model);
        Task PatchRole(AuthPatchModel model);
        Task UpdateRole(TenantRole model);
        Task<UserMembershipDto> GetMembership(IdentityUser user);

        Task<bool> IsTenantExistedByName(string tenantName);
    }
}
