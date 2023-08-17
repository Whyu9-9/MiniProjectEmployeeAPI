using AutoMapper;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;
using EmployeeApi.Models;

namespace employee.Profiles
{
	public class EmployeeProfile : Profile
	{
		public EmployeeProfile()
		{
			CreateMap<EmployeeForCreationDto, Employee>()
				.ForMember(
					dest => dest.Company,
					opt => opt.MapFrom(src => src.Company)
				).ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
                ).ForMember(
                    dest => dest.Phone,
                    opt => opt.MapFrom(src => src.Phone)
                ).ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => src.Address)
                );

            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                ).ForMember(
                    dest => dest.Company,
                    opt => opt.MapFrom(src => src.Company)
                ).ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
                ).ForMember(
                    dest => dest.Phone,
                    opt => opt.MapFrom(src => src.Phone)
                ).ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => src.Address)
                );
        }
	}
}

