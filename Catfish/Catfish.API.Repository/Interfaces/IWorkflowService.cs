using Catfish.API.Repository.Models.Workflow;
using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IWorkflowService
    {
        Task<List<Workflow>> GetWorkflows();
        Task<Workflow?> GetWorkFlow(Guid id);
        Task<WorkflowDbRecord?> GetWorkflowDbRecord(Guid id);
        Workflow GetWorkFlowDetails(Guid id);
        string GetLoggedUserEmail();
        List<WorkflowUser> GetPiranhaUsers();
        bool ExecuteTriggers(Guid workflowId, Guid actionId, Guid buttonId);
    }
}
