namespace EmployeeApi.DTOs.Incoming;

public partial class EmployeeForCreationDto
{
    public string? Company { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }
}
