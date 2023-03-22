using AutoMapper;

namespace Catfish.API.Repository.Models.Forms
{
    public class FormMapper : Profile
    {
        public FormMapper()
        {
            CreateMap<FormData, FormDataDto>();
            CreateMap<FormTemplate, FormTemplateDto>();
        }
        
    }
}
