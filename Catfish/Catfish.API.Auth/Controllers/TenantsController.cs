using AutoMapper;
using Catfish.API.Auth.Interfaces;
using Catfish.API.Auth.Models;
using CatfishExtensions.Constants;
using CatfishExtensions.DTO;
using CatfishExtensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catfish.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    public class TenantsController : ControllerBase
    {
        private readonly AuthDbContext _db;
        private readonly ILogger<TenantsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public TenantsController(AuthDbContext db, ILogger<TenantsController> logger, IMapper mapper, IAuthService authService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<TenantInfo>>> GetTenants(int offset = 0, int max = int.MaxValue, bool includeRoles = false)
        {
            IQueryable<Tenant> query = includeRoles ? _db.Tenants.Include(t => t.Roles) : _db.Tenants;
            var tenants = await query.OrderBy(t => t.Name).Skip(offset).Take(max).ToListAsync();
            return Ok(tenants.Select(rec => _mapper.Map<TenantInfo>(rec)).ToList());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TenantInfo>> GetTenant(Guid id)
        {
            var tenant = await _db.Tenants
                .Include(t => t.Roles)
                .ThenInclude(r => r.Users)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync(); 
            
            return (tenant== null) ? NotFound() : Ok(_mapper.Map<TenantInfo>(tenant));
        }

        [HttpGet("by-name/{name}")]
        [Authorize]
        public async Task<ActionResult<TenantInfo>> GetTenant(string name)
        {
            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Name == name);
            return (tenant == null) ? NotFound() : Ok(_mapper.Map<TenantInfo>(tenant));
        }

        [HttpPut]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> PutTenant(TenantInfo dto)
        {
            Tenant tenant = _mapper.Map<Tenant>(dto);
            _db.Entry(tenant).State= EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.TenantRoles.Any(t => t.Id == dto.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<TenantInfo>> PostTenant(TenantInfo dto)
        {
            try
            {
                Tenant model = _mapper.Map<Tenant>(dto);

                //Mr March 3 2023: Add checking to only added tenant when the given tenant's name is not existed yet in the db
                bool tenantExisted =await  _authService.IsTenantExistedByName(model.Name);
               
                if (!tenantExisted) { 
                    _db.Tenants.Add(model);
                    await _db.SaveChangesAsync();
                  }
                return Ok(_mapper.Map<TenantInfo>(model));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_db.TenantRoles.Any(t => t.Id == dto.Id))
                    return BadRequest("Tenant already exist in the system.");
                else
                    throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> DeleteTenant(Guid id)
        {
            try
            {
                var model = await _db.Tenants.FirstOrDefaultAsync(t => t.Id == id);
                if (model == null)
                    return NotFound();

                if (model.Roles.Any())
                    return BadRequest("Cannot delete Tenants that contain roles");

                _db.Remove(model);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPatch]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> PatchTenant(AuthPatchModel dto)
        {
            try
            {
                await _authService.PatchTenant(dto);
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
