namespace Catfish.API.Repository.Models.Entities
{
    public class Entity
    {
        public Guid Guid { get; set; }
        public Guid TemplateId { get; set; }
        public EntityTemplate Template { get; set; }
        public ICollection<Entity> SubjectEntities { get; set; } = new List<Entity>();
        public ICollection<Entity> ObjectEntities { get; set; } = new List<Entity>();
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();

        [JsonIgnore]
        public string? SerializedData { get; set; }

        [NotMapped]
        public ICollection<FormData> Data { get; set; } = new List<FormData>();
    }
}
