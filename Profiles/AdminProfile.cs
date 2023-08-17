using AutoMapper;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;
using EmployeeApi.Models;

namespace employee.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminForCreationDto, Admin>()
                .ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(src => src.Username)
                ).ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => src.Password)
                ).ForMember(
                    dest => dest.Salt,
                    opt => opt.MapFrom(src => src.Salt)
                );

            CreateMap<AdminForEditDto, Admin>()
                .ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(src => src.Username)
                );

            CreateMap<Admin, AdminDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                ).ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(src => src.Username)
                );
        }
    }
}

