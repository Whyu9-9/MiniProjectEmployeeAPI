namespace EmployeeApi.DTOs.Outgoing;

public class EmployeeDto
{
    public uint Id { get; set; }

    public string? Company { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }
}
