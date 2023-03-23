using AutoMapper;
using AutoMapper.Extensions.EnumMapping;

namespace Catfish.API.Repository.Models.Entity
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<EntityData, EntityDataDto>().ReverseMap();
            // CreateMap<EntityDataDto, EntityData>();
            //  .ConvertUsingEnumMapping(opt => opt

            //       .MapValue(Source.First, Destination.Default)

            CreateMap<EntityTemplate, EntityTemplateDto>().ReverseMap();// ConvertUsingEnumMapping(opt => opt.MapValue(EntityTemplate.State, EntityTemplateDto.State)).ReverseMap(); 
          // CreateMap<EntityTemplateDto, EntityTemplate>().ForMember(dest => dest.Updated, opt => opt.MapFrom(src => (src.Updated != null))).ReverseMap();

            CreateMap<Relationship, RelationshipDto>().ReverseMap();
           // CreateMap<RelationshipDto, Relationship>();

           // CreateMap<EntityData, EntityEntry>();
           // CreateMap<EntityEntry, EntityData>();
        }
    }
}
