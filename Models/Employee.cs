namespace EmployeeApi.Models;

public partial class Employee
{
    public uint Id { get; set; }

    public string? Company { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
