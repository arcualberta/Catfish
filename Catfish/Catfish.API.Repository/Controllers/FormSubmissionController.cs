using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.FormSubmissions.Root)]
    public class FormSubmissionController : ControllerBase
    {
        private readonly RepoDbContext _context;

        public FormSubmissionController(RepoDbContext context)
        {
            _context = context;
        }
        // GET: api/<FormSubmissionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guid>>> GetFormSubmissions()
        {
          if (_context.FormData == null)
          {
              return NotFound();
          }
            return await _context.FormData.Select(fd => fd.Id).ToListAsync();
        }

        // GET: api/Forms/5
        // GET api/<FormSubmissionController>/5
        [HttpGet("{id}")]
        public async Task<FormData?> Get(Guid id)
         {
            FormData? formData = await _context.FormData!.FirstOrDefaultAsync(fd => fd.Id == id);
            return formData;
        }

        // POST api/<FormSubmissionController>
        [HttpPost]
        public async Task<Guid> Post([FromBody] FormData value)
        {
            _context.FormData?.Add(value);
            await _context.SaveChangesAsync();
            return value.Id;
        }

        // PUT api/<FormSubmissionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] FormData value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _context.Entry(value).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!FormSubmissionExists(id))
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

        // PATCH api/<FormSubmissionController>/5
        [HttpPatch("{id}")]
        public void Patch(Guid id, [FromBody] FormData value)
        {
            throw new NotImplementedException();
        }


        // DELETE api/<FormSubmissionController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        #region Private methods
        private bool FormSubmissionExists(Guid id)
        {
            return (_context.FormData?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
