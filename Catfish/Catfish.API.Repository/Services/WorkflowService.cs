using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Forms;
using Catfish.API.Repository.Models.Workflow;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Catfish.API.Repository.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly RepoDbContext _context;

        public WorkflowService(RepoDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowDbRecord?> GetWorkflowDbRecord(Guid id)
        {
            return await _context.Workflows.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Workflow?> GetWorkFlow(Guid id)
        {
            WorkflowDbRecord workflowRecord = await _context.Workflows.Where(w => w.Id == id).FirstOrDefaultAsync();

            if (workflowRecord == null)
                return null;

            Workflow wrkFlow = workflowRecord.Workflow;

            return wrkFlow;

        }

        public async Task<List<Workflow>> GetWorkflows()
        {
            List<WorkflowDbRecord> workflowRecords = await _context.Workflows.ToListAsync();
            return workflowRecords.Select(wr => wr.Workflow).ToList();
        }
    }
}
