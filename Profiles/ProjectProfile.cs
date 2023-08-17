using AutoMapper;
using EmployeeApi.Models;
using EmployeeApi.Models.DTOs.Incoming;
using EmployeeApi.Models.DTOs.Outgoing;

namespace employee.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectForCreationDto, Project>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
                ).ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
                );

            CreateMap<Project, ProjectDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                ).ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
                ).ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
                ).ForMember(
                    dest => dest.ProjectEmployees,
                    opt => opt.MapFrom(src => src.ProjectEmployees.Select(pe => pe.Employee))
                );

            CreateMap<ProjectEmployeeForCreationDto, ProjectEmployee>()
                .ForMember(
                    dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId)
                ).ForMember(
                    dest => dest.EmployeeId,
                    opt => opt.MapFrom(src => src.EmployeeId)
                );
        }
    }
}

