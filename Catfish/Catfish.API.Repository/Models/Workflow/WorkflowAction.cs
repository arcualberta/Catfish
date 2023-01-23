namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowAction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public eButtonType ButtonType { get; set; }
        public string ButtonLabel { get; set; }
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();
        public List<WorkflowPermission> Permissions { get; set; } = new List<WorkflowPermission>();
        public object FrontEndStoreAction { get; set; }
        public object FrontEndViewTransition { get; set; }
    }
}
