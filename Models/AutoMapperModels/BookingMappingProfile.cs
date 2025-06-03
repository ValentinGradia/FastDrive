using AutoMapper;
using FastDrive.Models.DTO;

namespace FastDrive.Models.AutoMapperModels
{
    public class BookingMappingProfile : Profile
    {
        //To convert an object to DTO and reverse
        public BookingMappingProfile()
        {
            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.PatentCar, opt => opt.MapFrom(src => src.CarPatent))
                .ForMember(dest => dest.DateStart, opt => opt.MapFrom(src => src.DateStart))
                .ForMember(dest => dest.DateEnd, opt => opt.MapFrom(src => src.DateEnd))
                .ReverseMap();
        }
    }
}
