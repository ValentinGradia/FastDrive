using AutoMapper;
using FastDrive.Models.DTO;

namespace FastDrive.Models.AutoMapperModels
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            //Creating to show only the username and password in the cases user wants
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}
