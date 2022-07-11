using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catfish.API.Repository;
using Catfish.API.Repository.Models.Form;

namespace Catfish.API.Repository.Controllers
{
    [Route(Routes.Forms.Root)]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly RepoDbContext _context;
        private readonly IFormService _formService;

        public FormsController(RepoDbContext context, IFormService formService)
        {
            _context = context;
            _formService = formService;
        }

        // GET: api/Forms
        [HttpGet(Routes.Forms.GetFieldTemplates)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Field>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetFieldTemplates()
        {
            try
            {
                var fieldTemplates = await _formService.GetTFieldemplates();
                return new JsonResult(fieldTemplates);
            }
            catch(AccessDeniedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }


        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Form>>> GetForms()
        {
          if (_context.Forms == null)
          {
              return NotFound();
          }
            return await _context.Forms.ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Form>> GetForm(Guid id)
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
        public async Task<IActionResult> PutForm(Guid id, Form form)
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
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Forms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Form>> PostForm(Form form)
        {
          if (_context.Forms == null)
          {
              return Problem("Entity set 'RepoDbContext.Forms'  is null.");
          }
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForm", new { id = form.Id }, form);
        }

        // DELETE: api/Forms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(Guid id)
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

            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormExists(Guid id)
        {
            return (_context.Forms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
