namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowPermission
    {
        public Guid Id { get; set; }
        public WorkflowState? CurrentState { get; set; }
        public WorkflowState? NewState { get; set; }
        public bool IsOwnerAuthorized { get; set; }
        public string[] AuthorizedDomains { get; set; } 
        public Role[] AuthorizedRoles { get; set; }
    }
}
