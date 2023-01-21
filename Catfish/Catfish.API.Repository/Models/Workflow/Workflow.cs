namespace Catfish.API.Repository.Models.Workflow
{
    public class Workflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid EntityTemplateId { get; set; }
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();

        public List<WorkflowAction>  Actions { get; set; } = new List<WorkflowAction>();
        public List<WorkflowState>  States { get; set; } = new List<WorkflowState>();
        public List<Role> Roles { get;set; } = new List<Role>();
        public List<EmailTemplate> EmailTemplates { get; set; }= new List<EmailTemplate>();
        public object Popups { get; set; }
    }
}
