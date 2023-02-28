using AutoMapper;
using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;

namespace Catfish.API.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(AuthDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task PatchTenant(AuthPatchModel model)
        {
            var tenant = await _db.Tenants.Include(t => t.Roles).SingleOrDefaultAsync(t => t.Id == model.ParentId);
            if (tenant == null)
                throw new CatfishException("Tenant not found") { HttpStatusCode = HttpStatusCode.NotFound };

            foreach(var roleName in model.DeleteChildren)
            {
                var role = tenant.Roles.SingleOrDefault(r => r.Name == roleName);
                if(role != null)
                {
                    if (_db.TenantUsers.Any(u => u.RoleId == role.Id))
                        throw new CatfishException("Cannot delete rolse that contains users");

                    tenant.Roles.Remove(role);
                }
            }

            foreach(var roleName in model.NewChildren)
            {
                //Create the new role in the tenant only if it does not exist.
                if (!tenant.Roles.Any(r => r.Name == roleName))
                {
                    tenant.Roles.Add(new TenantRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        TenantId = tenant.Id
                    });

                    _db.TenantRoles.Add(tenant.Roles.Last());
                }
            }
        }

        public async Task PatchRole(AuthPatchModel model)
        {
            var role = await _db.TenantRoles.Include(r => r.Users).SingleOrDefaultAsync(r => r.Id == model.ParentId);
            if (role == null)
                throw new CatfishException($"Role not found") { HttpStatusCode= HttpStatusCode.NotFound };

            foreach(var userName in model.DeleteChildren)
            {
                var user = role.Users.FirstOrDefault(u => u.UserName == userName);
                if(user != null)
                    role.Users.Remove(user);
            }

            foreach(var username in model.NewChildren)
            {
                if(role.Users.FirstOrDefault(u => u.UserName == username) == null)
                {
                    //Throw an error if the specified user not exist in the global user-account system
                    var userAccount = await _userManager.FindByNameAsync(username);
                    if (userAccount == null)
                        throw new CatfishException($"{username} has no user account.") { HttpStatusCode = HttpStatusCode.NotFound };

                    role.Users.Add(new TenantUser()
                    {
                        Id = Guid.NewGuid(),
                        IdentityUserId = userAccount.Id,
                        UserName = username,
                        Email = userAccount.Email,
                        Role = role,
                        RoleId = role.Id
                    });

                    _db.TenantUsers.Add(role.Users.Last());
                }
            }
        }

        public async Task UpdateRole(TenantRole model)
        {
            if(model.TenantId == Guid.Empty)
                model.TenantId = await _db.TenantRoles.Where(r => r.Id == model.Id).Select(r => r.TenantId).FirstOrDefaultAsync(); ;
            _db.Entry(model).State= EntityState.Modified;
        }

        public async Task<UserMembership> GetMembership(IdentityUser user)
        {
            UserMembership membership = new UserMembership();

            if (user == null)
                return membership;

            membership.User = new UserInfo() { Id = Guid.Empty, IdentityUserId = user.Id, UserName = user.UserName, Email = user.Email };

            //Get the list of Tenant-User objects where this user is assciated with
            var userTenancies = await _db.TenantUsers
                .Include(u => u.Role)
                .ThenInclude(r => r.Tenant)
                .Where(u => u.IdentityUserId == user.Id)
                .ToListAsync();

            foreach (var tenancyUser in userTenancies)
            {
                var tenantInfo = membership.Tenancy.FirstOrDefault(t => t.Id == tenancyUser.Role.TenantId);
                if (tenantInfo == null)
                {
                    tenantInfo = new TenantInfo() 
                    { 
                        Id = tenancyUser.Role.TenantId, 
                        Name = tenancyUser.Role.Tenant.Name, 
                        Description = tenancyUser.Role.Tenant.Description
                    };
                    membership.Tenancy.Add(tenantInfo);
                }

                var roleInfo = tenantInfo.Roles!.FirstOrDefault(r => r.Id == tenancyUser.RoleId);
                if (roleInfo == null)
                {
                    roleInfo = new TenantRoleInfo() 
                    { 
                        Id = tenancyUser.RoleId,
                        Name = tenancyUser.Role.Name, 
                        Description = tenancyUser.Role.Description,
                    };

                    tenantInfo.Roles.Add(roleInfo);
                }
            }

            return membership;
        }
    }
}
