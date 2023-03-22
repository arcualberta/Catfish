using AutoMapper;

namespace Catfish.API.Repository.Models.Entity
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<EntityData, EntityDataDto>();
            CreateMap<EntityDataDto, EntityData>();

            CreateMap<EntityTemplate, EntityTemplateDto>();
            CreateMap<EntityTemplateDto, EntityTemplate>();

            CreateMap<Relationship, RelationshipDto>();
            CreateMap<RelationshipDto, Relationship>();

            CreateMap<EntityData, EntityEntry>();
            CreateMap<EntityEntry, EntityData>();
        }
    }
}
