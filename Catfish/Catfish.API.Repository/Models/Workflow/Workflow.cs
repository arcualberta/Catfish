namespace Catfish.API.Repository.Models.Workflow
{
    public class Workflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid EntityTemplateId { get; set; }
        public List<WorkflowAction>  Actions { get; set; } = new List<WorkflowAction>();
        public List<WorkflowState>  States { get; set; } = new List<WorkflowState>();
        public List<WorkflowRole> Roles { get;set; } = new List<WorkflowRole>();
        public List<WorkflowTrigger> Triggers { get; set; } = new List<WorkflowTrigger>();
        public List<WorkflowEmailTemplate> EmailTemplates { get; set; }= new List<WorkflowEmailTemplate>();
        public List<WorkflowPopup> Popups { get; set; } = new List<WorkflowPopup>();
    }
}
