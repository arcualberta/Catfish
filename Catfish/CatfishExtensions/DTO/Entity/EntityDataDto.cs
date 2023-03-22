

using CatfishExtensions.Constants;

namespace CatfishExtensions.DTO.Entity
{
    public class EntityDataDto
    {
        public Guid Id { get; set; }
       public eEntityType EntityType { get; set; }

      
        public Guid TemplateId { get; set; }
        public EntityTemplateDto? Template { get; set; }

        public virtual List<RelationshipDto> SubjectRelationships { get; set; } = new List<RelationshipDto>();
        public virtual List<RelationshipDto> ObjectRelationships { get; set; } = new List<RelationshipDto>();

     
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

       public eState State { get; set; }
    }
}
