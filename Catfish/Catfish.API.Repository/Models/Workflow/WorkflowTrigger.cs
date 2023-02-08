namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowTrigger
    {
        public Guid Id { get; set; }
        public eTriggerType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TemplateId { get; set; }
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();
    }
}
