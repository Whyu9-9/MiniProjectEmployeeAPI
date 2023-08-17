namespace EmployeeApi.Models;

public partial class ProjectEmployee
{
    public uint Id { get; set; }

    public uint? ProjectId { get; set; }

    public uint? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
