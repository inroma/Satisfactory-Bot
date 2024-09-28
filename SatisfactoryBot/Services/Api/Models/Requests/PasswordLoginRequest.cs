namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class PasswordLoginRequest
{
    [JsonPropertyName("minimumPrivilegeLevel")]
    public string MinimumPrivilegeLevel { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
