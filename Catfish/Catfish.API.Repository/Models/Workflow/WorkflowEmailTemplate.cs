namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowEmailTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
