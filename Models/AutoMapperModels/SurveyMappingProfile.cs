using AutoMapper;
using FastDrive.Models.DTO;

namespace FastDrive.Models.AutoMapperModels
{
    public class SurveyMappingProfile : Profile
    {
        public SurveyMappingProfile()
        {
            CreateMap<Survey, SurveyDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ServiceCalification, opt => opt.MapFrom(src => src.ServiceCalification))
                .ReverseMap();
        }
    }
}
