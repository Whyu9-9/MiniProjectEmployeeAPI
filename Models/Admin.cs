using System.Text.Json.Serialization;

namespace EmployeeApi.Models;

public partial class Admin
{
    public uint Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [JsonIgnore]
    public string? Salt { get; set; }
}
