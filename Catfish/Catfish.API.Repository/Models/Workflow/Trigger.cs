namespace Catfish.API.Repository.Models.Workflow
{
    public class Trigger
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public eTriggerType eTriggerType { get; set; }
    }
}
