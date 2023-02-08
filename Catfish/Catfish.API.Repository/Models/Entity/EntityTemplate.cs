using Catfish.API.Repository.Models.Workflow;

namespace Catfish.API.Repository.Models.Entities
{
    public class EntityTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public eState State { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        [JsonIgnore]
        public string? SerializedEntityTemplateSettings { get; set; }

        [NotMapped]
        public EntityTemplateSettings? EntityTemplateSettings
        {
            get => SerializedEntityTemplateSettings == null ? null : JsonConvert.DeserializeObject<EntityTemplateSettings>(SerializedEntityTemplateSettings);
            set => SerializedEntityTemplateSettings = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public ICollection<FormTemplate> Forms { get; set; } = new List<FormTemplate>();

        public ICollection<WorkflowDbRecord> Workflows { get; set; } = new List<WorkflowDbRecord>();
    }
}
