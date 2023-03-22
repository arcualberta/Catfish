using AutoMapper;

namespace Catfish.API.Repository.Models.Forms
{
    public class FormMapper : Profile
    {
        public FormMapper()
        {
            CreateMap<FormData, FormDataDto>();
            CreateMap<FormDataDto, FormData>();
            CreateMap<FormTemplate, FormTemplateDto>();
            CreateMap<FormTemplateDto, FormTemplate>();
        }
        
    }
}
