﻿using Catfish.API.Repository.Interfaces;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Entities.Root)]
    public class EntitiesController : ControllerBase
    {
        private readonly RepoDbContext _context;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IEntityService _entityService;

        public EntitiesController(RepoDbContext context, IEntityTemplateService entityTemplateService, IEntityService entityService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
            _entityService = entityService;
        }
        // GET: api/<EntityTemplateController>
        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<Entity>>> Get()
        {
            if (_context.Entities == null)
            {
                return NotFound();
            }
            return await _context.Entities.ToListAsync();
            
        }

        // GET: api/Forms/5
        //   GET api/<EntitiesController>/5
        [HttpGet("{id}")]
        public async Task<Entity> Get(Guid id, bool includeRelationship=true)
        {
             if(includeRelationship)
                return await _context.Entities!.Include(e=>e.SubjectRelationships)
                                               .Include(e=>e.ObjectRelationships)
                                               .Include(e=>e.Template)
                                               .FirstOrDefaultAsync(fd => fd.Id == id);
             else
                return await _context.Entities!.FirstOrDefaultAsync(fd => fd.Id == id);
        }

        // POST api/<EntityTemplatesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Entity value)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                // var code = await _entityTemplateService.AddEntity(value);
                EntityTemplate template = _entityTemplateService.GetEntityTemplate(value.TemplateId);
                value.Template = template;

                var code = await _entityService.AddEntity(value);

                await _context.SaveChangesAsync();
               return StatusCode((int)code);
            }
            catch(Exception)
            {
                //TODO: Log the error in error log
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/<EntitiesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Entity value)
        {
            try
            {
                if (id != value.Id)
                    return BadRequest();

               // var code = await _entityTemplateService.UpdateEntity(value);
               _context.Entry(value).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(); // StatusCode((int) code);
            }
            catch(Exception)
            {
                //TODO: Log the error in error log
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

       // PATCH api/<EntitiesController>/5
        [HttpPatch("{id}")]
        public void Patch(Guid id, [FromBody] FormData value)
        {
            throw new NotImplementedException();
        }


       // DELETE api/<EntitiesController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        
    }
}