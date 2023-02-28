using AutoMapper;
using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Identity;

namespace Catfish.API.Auth.Models
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<Tenant, TenantInfo>();
            CreateMap<TenantInfo, Tenant>();

            CreateMap<TenantRole, TenantRoleInfo>();
            CreateMap<TenantRoleInfo, TenantRole>();

            CreateMap<TenantUser, UserInfo>();
            CreateMap<IdentityUser, UserInfo>();
        }
    }
}
