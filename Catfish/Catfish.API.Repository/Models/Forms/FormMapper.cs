using AutoMapper;

namespace Catfish.API.Repository.Models.Forms
{
    public class FormMapper : Profile
    {
        public FormMapper()
        {
            CreateMap<FormData, FormDataDto>().ReverseMap();
            //CreateMap<FormDataDto, FormData>();

            CreateMap<FormTemplate, FormTemplateDto>().ReverseMap();
           // CreateMap<FormTemplateDto, FormTemplate>();
        }
        
    }
}
