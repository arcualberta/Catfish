namespace Catfish.API.Repository.Models.Workflow
{
    public class Workflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();
    }
}
