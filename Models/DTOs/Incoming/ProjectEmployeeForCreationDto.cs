namespace employee.Models.DTOs.Incoming;

public partial class ProjectEmployeeForCreationDto
{
    public uint? ProjectId { get; set; }

    public List<uint>? EmployeeId { get; set; }

}