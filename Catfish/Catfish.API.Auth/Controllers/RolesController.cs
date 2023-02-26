using AutoMapper;
using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Catfish.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AuthDbContext _db;
        public readonly IMapper _mapper;
        public readonly IAuthService _authService;

        public RolesController(AuthDbContext db, IMapper mapper, IAuthService authService)
        {
            _db = db;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPut]
        public async Task<ActionResult> PutRole(TenantRoleInfo dto)
        {
            try
            {
                await _authService.UpdateRole(_mapper.Map<TenantRole>(dto));
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_db.TenantRoles.Any(r => r.Id == dto.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPatch]
        public async Task<ActionResult> PatchRole(AuthPatchModel dto)
        {
            try
            {
                await _authService.PatchRole(dto);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch(CatfishException ex)
            {
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
