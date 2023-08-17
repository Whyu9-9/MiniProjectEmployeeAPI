using EmployeeApi.Models.DTOs.Incoming;
using EmployeeApi.Models.DTOs.Outgoing;

namespace employee.Repository.EmployeeRepository
{
	public interface IProjectRepository
	{
        IEnumerable<ProjectDto> GetProject();
        ProjectDto? GetProjectById(uint id);
        ProjectDto PostProject(ProjectForCreationDto data);
        ProjectDto? PostProjectEmployee(ProjectEmployeeForCreationDto data);
        ProjectDto? PutProject(uint id, ProjectForCreationDto data);
        string? PutProjectEmployee(uint id, ProjectEmployeeForCreationDto data);
        ProjectDto? DeleteProject(uint id);
    }
}

