using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route("[controller]")]
    public class EntityTemplatesController : ControllerBase
    {
        private readonly RepoDbContext _context;

        public EntityTemplatesController(RepoDbContext context)
        {
            _context = context;
        }
        // GET: api/<EntityTemplateController>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Guid>>> GetFormSubmissions()
        //{
        //  if (_context.FormData == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.FormData.Select(fd => fd.Id).ToListAsync();
        //}

        // GET: api/Forms/5
        // GET api/<FormSubmissionController>/5
        //[HttpGet("{id}")]
        //public async Task<FormData?> Get(Guid id)
        //{
        //    FormData? formData = await _context.FormData.FirstOrDefaultAsync(fd => fd.Id == id);
        //    return formData;
        //}

        // POST api/<EntityTemplatesController>
        [HttpPost]
        public async Task<Guid> Post([FromBody] EntityTemplate value)
        {
            try
            {
                if (EntityTemplateExists(value.Id))
                {
                    throw new Exception("This object has been existed in the database!");
                }
                if (ModelState.IsValid && !EntityTemplateExists(value.Id))
                {
                    _context.EntityTemplates?.Add(value);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return value.Id;
        }

        // PUT api/<EntityTeplatesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EntityTemplate value)
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
                if (!EntityTemplateExists(id))
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
