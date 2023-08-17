using System.Text.Json.Serialization;

namespace EmployeeApi.DTOs.Incoming;

public class AdminForCreationDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [JsonIgnore]
    public string? Salt { get; set; }
}
