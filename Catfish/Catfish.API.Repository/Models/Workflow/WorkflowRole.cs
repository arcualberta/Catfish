namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<WorkflowUser> Users { get; set; } = new List<WorkflowUser>();
    }
}
