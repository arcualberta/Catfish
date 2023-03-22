using AutoMapper;
using Catfish.API.Repository.Interfaces;
using System.Linq;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.EntityTemplates.Root)]
    public class EntityTemplatesController : ControllerBase
    {
        private readonly RepoDbContext _context;

        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IMapper _mapper;

        public EntityTemplatesController(RepoDbContext context, IEntityTemplateService entityTemplateService, IMapper mapper)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
            _mapper = mapper;
        }
        // GET: api/<EntityTemplateController>
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<IEnumerable<TemplateEntry>>> Get()
        {
            if (_context.EntityTemplates == null)
            {
                return NotFound();
            }
            return await _context.EntityTemplates!
                .Where(et=>et.State != eState.Deleted)
                .OrderBy(et => et.Name)
                .Select(te => new TemplateEntry() { Id = te.Id, Name = te.Name ?? te.Id.ToString() })
                .ToListAsync();
        }

        // GET: api/Forms/5
        //   GET api/<EntityTemplatesController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<EntityTemplateDto?> Get(Guid id, bool includeForms = true)
        {

            var entityTemplates = (dynamic)null;
            if (includeForms)
            {
                entityTemplates = await _context.EntityTemplates!.Include(et => et.Forms).FirstOrDefaultAsync(fd => fd.Id == id);
            }
            else
            {
                entityTemplates= await _context.EntityTemplates!.FirstOrDefaultAsync(fd => fd.Id == id);
            }

            return Ok(_mapper.Map<EntityTemplateDto>(entityTemplates));
        }

        // POST api/<EntityTemplatesController>
        [HttpPost]
        [Authorize(Roles ="SysAdmin")]
        public async Task<IActionResult> Post([FromBody] EntityTemplateDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if(value.Created == DateTime.MinValue)
                    value.Created= DateTime.Now;

                value.Updated = DateTime.Now;

                EntityTemplate template = _mapper.Map<EntityTemplate>(value);

                var code = await _entityTemplateService.AddEntity(template);
                await _context.SaveChangesAsync();
                return StatusCode((int)code);
            }
            catch(Exception)
            {
                //TODO: Log the error in error log
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/<EntityTeplatesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EntityTemplateDto value)
        {
            try
            {
                if (id != value.Id)
                    return BadRequest();

                value.Updated = DateTime.Now;
                EntityTemplate template = _mapper.Map<EntityTemplate>(value);
                var code = await _entityTemplateService.UpdateEntity(template);
                await _context.SaveChangesAsync();
                return StatusCode((int) code);
            }
            catch(Exception)
            {
                //TODO: Log the error in error log
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //// PATCH api/<FormSubmissionController>/5
        [HttpPatch("{id}")]
        public ActionResult Patch(Guid id, [FromBody] FormTemplateDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                EntityTemplate template = _entityTemplateService.GetEntityTemplate(id);

                if (template == null)
                    return BadRequest();

                //Add new formTemplate to existing entityTemplate
                FormTemplate formTemplate = _mapper.Map<FormTemplate>(value);
                template.Forms.Add(formTemplate);
                _context.Entry(template).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        // DELETE api/<FormSubmissionController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.EntityTemplates == null)
            {
                return NotFound();
            }
            var entityTemplate = await _context.EntityTemplates.FindAsync(id);
            if (entityTemplate == null)
            {
                return NotFound();
            }

            entityTemplate.State = eState.Deleted;
            _context.Entry(entityTemplate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("change-state/{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> ChangeState(Guid id, [FromBody] eState newState)
        {
            if (_context.EntityTemplates == null)
            {
                return NotFound();
            }
            var entityTemplate = await _context.EntityTemplates.FindAsync(id);
            if (entityTemplate == null)
            {
                return NotFound();
            }

            entityTemplate.State =newState;
            _context.Entry(entityTemplate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();

        }

        #region Private methods
        private bool EntityTemplateExists(Guid id)
        {
            return (_context.Entities?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
