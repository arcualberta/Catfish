using Catfish.API.Repository.Models.Workflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Workflow.Root)]
    public class WorkflowController : ControllerBase
    {
        private readonly RepoDbContext _context;
        public WorkflowController(RepoDbContext context)
        {
            _context = context;
        }
        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workflow>>> Get()
        {
            if (_context.Workflows == null)
            {
                return NotFound();
            }
            return await _context.Workflows.ToListAsync();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Workflow>> GetWorkflow(Guid id)
        {
            if (_context.Workflows == null)
            {
                return NotFound();
            }
            var workflow = await _context.Workflows.FindAsync(id);

            if (workflow == null)
            {
                return NotFound();
            }

            return workflow;
        }

        // PUT: api/Forms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkflow(Guid id, Workflow workflow)
        {
            if (id != workflow.Id)
            {
                return BadRequest();
            }

            _context.Entry(workflow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                
              return StatusCode(500);
            
            }

            return Ok();
        }

        // POST: api/Forms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Workflow>> PostWorkflow(Workflow workflow)
        {
            try
            {

                if (_context.Workflows == null)
                {
                    return Problem("Entity set 'RepoDbContext.Forms'  is null.");
                }
                _context.Workflows.Add(workflow);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                 return StatusCode(500);
            }
        }

        // DELETE: api/workflow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Workflows == null)
            {
                return NotFound();
            }
            var workflow = await _context.Workflows.FindAsync(id);
            if (workflow == null)
            {
                return NotFound();
            }

           //THIS MIGHT NEED TO CHANGE -- WILL IT BE PERMANENTLY DELETED OR JUST CHANGE ITS STATE/STATUS TO DELETED!!!!!!!!
            _context.Entry(workflow).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok();
        }
       
    }
}
