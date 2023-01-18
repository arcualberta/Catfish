namespace Catfish.API.Repository.Models.Workflow
{
    public class WorkflowDbRecord
    {

        public Guid Id
        {
            get { return Id; } set { value = Workflow.Id; }
        }
        public string Name { get; set; }
        public string Description { get; set; }

       
        public string SerializedWorkflow { 
            get; set;
        }

        [NotMapped]
        public Workflow Workflow {
            get => SerializedWorkflow == null ? null : JsonConvert.DeserializeObject<Workflow>(SerializedWorkflow);
            set => SerializedWorkflow = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public WorkflowDbRecord()
        {
            Workflow = new Workflow();
            SerializedWorkflow = System.Text.Json.JsonSerializer.Serialize(Workflow);
        }

        public WorkflowDbRecord(Workflow _workflow)
        {
            Workflow = _workflow;
            SerializedWorkflow = System.Text.Json.JsonSerializer.Serialize(Workflow);
        }


    }
}
