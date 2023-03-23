using AutoMapper;
using CatfishExtensions.DTO;
using Microsoft.AspNetCore.Identity;

namespace Catfish.API.Auth.Models
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<Tenant, TenantInfo>().ReverseMap();
            CreateMap<TenantRole, TenantRoleInfo>().ReverseMap();
            CreateMap<TenantUser, UserInfo>().ReverseMap();
        }
    }
}
