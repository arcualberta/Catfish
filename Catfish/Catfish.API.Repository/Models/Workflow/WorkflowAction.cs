namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowAction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? FormTemplateId { get; set; }
        public eFormView FormView { get; set; }
        public List<Button> Buttons { get; set; } = new List<Button>();
        public List<Authorization> Authorizations { get; set; } = new List<Authorization>();
    }
}
