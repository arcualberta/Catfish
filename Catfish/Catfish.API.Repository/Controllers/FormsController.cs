
using AutoMapper;
using CatfishExtensions.DTO.Forms;

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Forms.Root)]
    public class FormsController : ControllerBase
    {
        private readonly RepoDbContext _context;
        private readonly IMapper _mapper;

        public FormsController(RepoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Forms
        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<IEnumerable<FormEntry>>> Get()
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            return await _context.Forms
                .Where(form=>form.Status != eState.Deleted)
                .OrderBy(form => form.Name)
                .Select(form => new FormEntry() { Id = form.Id, Name = form.Name?? form.Id.ToString() })
                .ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<FormTemplateDto>> GetForm(Guid id)
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            var form = await _context.Forms.FindAsync(id);

            if (form == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FormTemplateDto>(form));
        }

        // PUT: api/Forms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> PutForm(Guid id, FormTemplateDto formDto)
        {
            if (id != formDto.Id)
            {
                return BadRequest();
            }

            FormTemplate form = _mapper.Map<FormTemplate>(formDto);
            form.Updated = DateTime.Now;

            _context.Entry(form).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!FormExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return Ok();
        }

        // POST: api/Forms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<FormTemplate>> PostForm(FormTemplateDto formDto)
        {
            try
            {

                if (_context.Forms == null)
                {
                    return Problem("Entity set 'RepoDbContext.Forms'  is null.");
                }

                FormTemplate form = _mapper.Map<FormTemplate>(formDto);
                form.Created = DateTime.Now;
                form.Updated = DateTime.Now;

                _context.Forms.Add(form);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                if (FormExists(formDto.Id))
                {
                    return BadRequest();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

        // DELETE: api/Forms/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            form.Status = eState.Deleted;
            _context.Entry(form).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("change-state/{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> ChangeState(Guid id, [FromBody] eState newState)
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            var form = await _context.Forms.Where(it => it.Id == id).FirstOrDefaultAsync();
            if (form == null)
            {
                return NotFound();
            }

            form.Status = newState;
            _context.Entry(form).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();

        }
        private bool FormExists(Guid id)
        {
            return (_context.Forms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
