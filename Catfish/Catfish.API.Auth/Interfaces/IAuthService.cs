using Catfish.API.Auth.Models;
using CatfishExtensions.DTO;

namespace Catfish.API.Auth.Interfaces
{
    public interface IAuthService
    {
        Task PatchTenant(AuthPatchModel model);
        Task PatchRole(AuthPatchModel model);
        Task UpdateRole(TenantRole model);
    }
}
