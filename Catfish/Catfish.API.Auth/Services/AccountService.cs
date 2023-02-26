using AutoMapper;
using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Catfish.API.Auth.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext _db;
        private readonly string _privateKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _jwtTokenLifeInMinutes;
        private readonly IAuthService _authService;
        public readonly IMapper _mapper;

        public AccountService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AuthDbContext db, IAuthService authService, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _db = db;
            _authService = authService;
            _mapper = mapper;

            _privateKey = _configuration["JwtConfig:RsaPrivateKey"]; // LoadJwtPrivateKeyFromFile();
            _issuer = _configuration["JwtConfig:Issuer"];
            _audience = _configuration["JwtConfig:Audience"];
            _jwtTokenLifeInMinutes = _configuration.GetValue("JwtConfig:TokenExpiryDurationInMinutes", 60);
        }

        #region Public Methods

        public async Task Seed(RegistrationModel model)
        {
            if (await _userManager.Users.CountAsync() > 0)
                throw new CatfishException("Accounts already exist.") { HttpStatusCode = HttpStatusCode.BadRequest };

            if (!model.SystemRoles.Where(r => r.ToLower() == "admin").Any())
                model.SystemRoles.Add("Admin");

			await CreateUser(model);
        }

        public async Task CreateUser(RegistrationModel model)
        {
            if(await _userManager.FindByEmailAsync(model.Email) != null)
				throw new CatfishException("Account already exist.") { HttpStatusCode = HttpStatusCode.BadRequest };

            if(model.Password != model.ConfirmPassword)
				throw new CatfishException("Password and Confirm Password must match.") { HttpStatusCode = HttpStatusCode.BadRequest };

            var identity = new IdentityUser { UserName = model.Username, Email= model.Email };
            if(!(await _userManager.CreateAsync(identity, model.Password)).Succeeded)
				throw new CatfishException("Could not create the user.") { HttpStatusCode = HttpStatusCode.InternalServerError };

            foreach(var role in model.SystemRoles)
            {
                if (await _roleManager.FindByNameAsync(role) == null && !(await _roleManager.CreateAsync(new IdentityRole() { Name = role })).Succeeded)
                    throw new CatfishException($"{role} could not be created.") { HttpStatusCode = HttpStatusCode.InternalServerError };

                if (!(await _userManager.AddToRoleAsync(identity, role)).Succeeded)
                    throw new CatfishException($"{role} could not be granted.") { HttpStatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task ChangePassword(ChangePasswordModel model, bool isAdmin)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                throw new CatfishException("User not found.") { HttpStatusCode = HttpStatusCode.NotFound };

            if (!isAdmin)
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                    throw new CatfishException("Current password is required.") { HttpStatusCode = HttpStatusCode.Unauthorized };

                //Check if the current password is correct
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    throw new CatfishException("Incorrect current password") { HttpStatusCode = HttpStatusCode.Unauthorized };
            }

            if (model.NewPassword != model.ConfirmPassword)
                throw new CatfishException("New password and confirm password do not match.") { HttpStatusCode = HttpStatusCode.BadRequest };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
                throw new CatfishException(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description))) { HttpStatusCode = HttpStatusCode.BadRequest };
        }

        public async Task DeleteUser(string username)
        {
            //Removing all associations of the user with all roles within all groups
            var memberships = await _db.TenantUsers.Where(u => u.UserName == username).ToListAsync();
            foreach (var membership in memberships)
                _db.TenantUsers.Remove(membership);
            await _db.SaveChangesAsync();

            //Removing the user account from the system
            var user = await _userManager.FindByNameAsync(username);
            var systemRolesForUser = await _userManager.GetRolesAsync(user);

            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var role in systemRolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = await _userManager.RemoveFromRoleAsync(user, role);
                }

                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }
        }

        public async Task<string> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var membership = await _authService.GetMembership(user);

                string token = GetSignedToken(user.UserName, userRoles, membership, "", DateTime.Now.AddMinutes(_jwtTokenLifeInMinutes));
                return token;
            }

            throw new CatfishException("Login failed") { HttpStatusCode = HttpStatusCode.Unauthorized };
        }


        public async Task<IList<UserInfo>> GetUsers(int offset = 0, int max = int.MaxValue)
        {
            return await _userManager.Users
                .OrderBy(u => u.UserName)
                .Skip(offset).Take(max)
                .Select(u => _mapper.Map<UserInfo>(u))
                .ToListAsync();
        }

        #endregion

        #region Private Methods
        private string LoadJwtPrivateKeyFromFile()
        {
            if (!string.IsNullOrEmpty(_configuration["JwtConfig:RsaPrivateKey"]))
            {
                List<string> key_lines = File.ReadAllLines(_configuration["JWT:Asymmetric:PrivateKey"]).ToList();
                if (key_lines.First().IndexOf("private key", StringComparison.OrdinalIgnoreCase) > 0)
                    key_lines.RemoveAt(0);

                if (key_lines.Last().IndexOf("private key", StringComparison.OrdinalIgnoreCase) > 0)
                    key_lines.RemoveAt(key_lines.Count - 1);

                string key = string.Join("", key_lines);
                return key;
            }
            return "";
        }

        private string GetSignedToken(string userName, IList<string> userRoles, UserMembership membership, string userData, DateTime expiresAt)
        {
            var authClaims = new List<Claim>
                {
					//new Claim(ClaimTypes.Name, userName),
					new Claim("username", userName),
                    new Claim("userdata", userData),
                    new Claim("membership", JsonConvert.SerializeObject(membership)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim("roles", userRole));
            //authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            return GetSignedToken(authClaims, expiresAt);
        }

        private string GetSignedToken(List<Claim> authClaims, DateTime expiresAt)
        {
            RSA rsa = RSA.Create();

            rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(_privateKey!), // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: authClaims,
                notBefore: DateTime.Now,
                expires: expiresAt,
                signingCredentials: signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        #endregion
    }
}
