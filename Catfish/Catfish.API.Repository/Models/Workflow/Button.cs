namespace Catfish.API.Repository.Models.Workflow
{
    public class Button
    {
        public Guid Id { get; set; }
        public eButtonTypes Type { get; set; }
        public string Label { get; set; }
        public Guid CurrentStateId { get; set; }
        public Guid NextStateId { get; set; }
        public Guid PopupId { get; set; }
        public List<Guid> Triggers { get; set; }
    }
}
