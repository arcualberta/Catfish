using CatfishExtensions.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Catfish.API.Auth.Models
{
    public class AuthDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantRole> TenantRoles { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            DbHelper.SetTablePrefix(builder, "CF_Auth_");
        }
    }
}
