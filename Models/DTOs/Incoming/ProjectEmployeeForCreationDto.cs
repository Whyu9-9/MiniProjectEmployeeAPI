namespace EmployeeApi.Models.DTOs.Outgoing;

public partial class ProjectEmployeeForCreationDto
{
    public uint? ProjectId { get; set; }

    public uint? EmployeeId { get; set; }
}
