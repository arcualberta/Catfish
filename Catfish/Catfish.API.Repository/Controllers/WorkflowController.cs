using Catfish.API.Repository.Interfaces;
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
        private readonly IWorkflowService _workflorSrv;
        public WorkflowController(RepoDbContext context, IWorkflowService workflorSrv)
        {
            _context = context;
            _workflorSrv = workflorSrv;
        }
        // GET: api/Forms
        [HttpGet]
        [Authorize(Roles ="SysAdmin")]
        public async Task<ActionResult<IEnumerable<Workflow>>> Get()
        {
            if (_context.Workflows == null)
            {
                return NotFound();
            }

            return await _workflorSrv.GetWorkflows();
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<Workflow>> GetWorkflow(Guid id)
        {
            if (_context.Workflows == null)
            {
                return NotFound();
            }
            var workflow = await _workflorSrv.GetWorkFlow(id);
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

            WorkflowDbRecord workflowRecord = await _workflorSrv.GetWorkflowDbRecord(id);

            workflowRecord.Workflow = workflow;
            workflowRecord.Updated = DateTime.Now;

            _context.Entry(workflowRecord).State = EntityState.Modified;

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
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult<Workflow>> PostWorkflow(Workflow workflow)
        {
            try
            {

                if (_context.Workflows == null)
                {
                    return Problem("Entity set 'RepoDbContext.Forms'  is null.");
                }
                WorkflowDbRecord workflowRecord = new WorkflowDbRecord(workflow);
                //NEED TO BE UPDATED
                workflowRecord.Name = workflow.Name;
                workflowRecord.Description = workflow.Description;
                workflowRecord.Created = DateTime.Now;
                workflowRecord.Updated = DateTime.Now;
                _context.Workflows.Add(workflowRecord);
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
        [Authorize(Roles = "SysAdmin")]
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
