using EmployeeApi.DTOs.Outgoing;

namespace EmployeeApi.Models.DTOs.Outgoing;

public class ProjectDto
{
    public uint Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<EmployeeDto> ProjectEmployees { get; set; } = new List<EmployeeDto>();
}
