

using CatfishExtensions.Constants;

namespace CatfishExtensions.DTO.Entity
{
    public class EntityData
    {
        public Guid Id { get; set; }
        public eEntityType EntityType { get; set; }

      
        public Guid TemplateId { get; set; }
       // public EntityTemplate? Template { get; set; }

        public virtual List<Relationship> SubjectRelationships { get; set; } = new List<Relationship>();
        public virtual List<Relationship> ObjectRelationships { get; set; } = new List<Relationship>();

     
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public eState State { get; set; }
    }
}
