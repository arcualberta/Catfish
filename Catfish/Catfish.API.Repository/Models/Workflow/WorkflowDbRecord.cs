using Newtonsoft.Json.Linq;

namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowDbRecord
    {
        [NotMapped]
        public Workflow Workflow { get; set; }

        public Guid Id
        {
            get => Workflow.Id;
            set => Workflow.Id = value;
        }
        public string Name {
            get => Workflow.Name; 
            set => Workflow.Name = value; 
        }
        public string Description
        {
            get => Workflow.Description;
            set => Workflow.Description = value;
        }

        public string SerializedWorkflow
        { 
            get => JsonConvert.SerializeObject(Workflow); 
            set => Workflow = string.IsNullOrEmpty(value) ? new Workflow() : JsonConvert.DeserializeObject<Workflow>(value!)!; 
        }

        public WorkflowDbRecord()
        {
            Workflow = new Workflow();
        }

        public WorkflowDbRecord(Workflow workflow)
        {
            Workflow = workflow;
        }
    }
}
