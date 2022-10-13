
namespace Catfish.API.Repository.Models.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public eEntityType EntityType { get; set; }

        [JsonIgnore]
        public string? SerializedData { get; set; }

        [NotMapped]
        public ICollection<FormData>? Data
        {
            get => SerializedData == null ? null : JsonConvert.DeserializeObject<List<FormData>>(SerializedData);
            set => SerializedData = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public Guid TemplateId { get; set; }
        public EntityTemplate? Template { get; set; }

        public virtual List<Relationship> SubjectRelationships { get; set; } = new List<Relationship>();
        public virtual List<Relationship> ObjectRelationships { get; set; } = new List<Relationship>();

        //[NotMapped]
       // public IFormFile[]? Files { get; set; }
       // public List<IFormFile>? Files { get; set; } = new List<IFormFile>();

    }

}
