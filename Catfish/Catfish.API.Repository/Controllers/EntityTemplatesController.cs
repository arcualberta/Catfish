using Catfish.API.Repository.Interfaces;
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

        public EntityTemplatesController(RepoDbContext context, IEntityTemplateService entityTemplateService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
        }
        // GET: api/<EntityTemplateController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemplateEntry>>> Get()
        {
            if (_context.FormData == null)
            {
                return NotFound();
            }
            return await _context.EntityTemplates!.Select(te => new TemplateEntry() { TemplateId = te.Id, TemplateName = te.Name ?? te.Id.ToString() }).ToListAsync();
            
        }

        // GET: api/Forms/5
        //   GET api/<EntityTemplatesController>/5
        [HttpGet("{id}")]
        public async Task<EntityTemplate?> Get(Guid id, bool includeForms = true)
        {
            if(includeForms)
                return await _context.EntityTemplates!.Include(et => et.Forms).FirstOrDefaultAsync(fd => fd.Id == id);
            else
                return await _context.EntityTemplates!.FirstOrDefaultAsync(fd => fd.Id == id);
        }

        // POST api/<EntityTemplatesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EntityTemplate value)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var code = await _entityTemplateService.AddEntity(value);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] EntityTemplate value)
        {
            try
            {
                if (id != value.Id)
                    return BadRequest();

                var code = await _entityTemplateService.UpdateEntity(value);
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
        //[HttpPatch("{id}")]
        //public void Patch(Guid id, [FromBody] FormData value)
        //{
        //    throw new NotImplementedException();
        //}


        //// DELETE api/<FormSubmissionController>/5
        //[HttpDelete("{id}")]
        //public void Delete(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        #region Private methods
        private bool EntityTemplateExists(Guid id)
        {
            return (_context.Entities?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
