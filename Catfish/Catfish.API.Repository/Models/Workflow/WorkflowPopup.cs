namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowPopup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<PopupButton> Buttons { get; set; } = new List<PopupButton>();
    }
}
