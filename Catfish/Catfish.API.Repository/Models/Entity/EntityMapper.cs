using AutoMapper;

namespace Catfish.API.Repository.Models.Entity
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<EntityData, EntityDataDto>();
            CreateMap<EntityTemplate, EntityTemplateDto>();
            CreateMap<Relationship, RelationshipDto>();
            CreateMap<EntityData, EntityEntry>();
        }
    }
}
