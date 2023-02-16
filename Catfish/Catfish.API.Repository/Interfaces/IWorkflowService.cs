using Catfish.API.Repository.Models.Workflow;
using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IWorkflowService
    {
        
        public Task<List<Workflow>> GetWorkflows();
        public Task<Workflow?> GetWorkFlow(Guid id);
        public Task<WorkflowDbRecord?> GetWorkflowDbRecord(Guid id);
        public Workflow GetWorkFlowDetails(Guid id);
        bool ExecuteTriggers(Guid workflowId, Guid actionId, Guid buttonId)
    }
}
