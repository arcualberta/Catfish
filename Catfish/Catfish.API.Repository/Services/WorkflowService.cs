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

        public async Task<WorkflowDbRecord> GetWorkflowDbRecord(Guid id)
        {
            return await _context.Workflows.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Workflow> GetWorklow(Guid id)
        {
            WorkflowDbRecord workflowRecord = _context.Workflows.Where(w => w.Id == id).FirstOrDefault();

            if (workflowRecord == null)
                return null;

            Workflow wrkFlow = workflowRecord.Workflow;

            return wrkFlow;

        }

        public async Task<List<Workflow>> GetWorkflows()
        {
            List<WorkflowDbRecord> workflowRecords = _context.Workflows.ToList();

            List<Workflow> workflows = new List<Workflow>();
            foreach (var workflowRecord in workflowRecords)
            {
                Workflow wf = workflowRecord.Workflow;
                workflows.Add(wf);
            }

            return workflows;
        }
    }
}
