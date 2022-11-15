
namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Forms.Root)]
    public class FormsController : ControllerBase
    {
        private readonly RepoDbContext _context;

        public FormsController(RepoDbContext context)
        {
            _context = context;
        }


        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormEntry>>> Get()
        {
            if (_context.Forms == null)
            {
                return NotFound();
            }
            return await _context.Forms.Select(form => new FormEntry() { Id = form.Id, Name = form.Name?? form.Id.ToString() }).ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormTemplate>> GetForm(Guid id)
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

            return form;
        }

        // PUT: api/Forms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForm(Guid id, FormTemplate form)
        {
            if (id != form.Id)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<FormTemplate>> PostForm(FormTemplate form)
        {
            try
            {

                if (_context.Forms == null)
                {
                    return Problem("Entity set 'RepoDbContext.Forms'  is null.");
                }
                _context.Forms.Add(form);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                if (FormExists(form.Id))
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

        private bool FormExists(Guid id)
        {
            return (_context.Forms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
