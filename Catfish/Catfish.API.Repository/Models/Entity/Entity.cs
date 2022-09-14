
namespace Catfish.API.Repository.Models.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public string? SerializedData { get; set; }

        [NotMapped]
        public ICollection<FormData> Data { get; set; } = new List<FormData>();

        public Guid TemplateId { get; set; }
        public EntityTemplate Template { get; set; }


        [InverseProperty("SubjectEntity")]
        public ICollection<SubjectRelationship> SubjectRelationships { get; set; } = new List<SubjectRelationship>();

        [InverseProperty("ObjectEntity")]
        public ICollection<ObjectRelationship> ObjectRelationships { get; set; } = new List<ObjectRelationship>();

    }
}
