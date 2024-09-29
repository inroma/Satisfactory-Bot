namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class PasswordLoginRequest : PasswordRequest
{
    [JsonPropertyName("minimumPrivilegeLevel")]
    public string MinimumPrivilegeLevel { get; set; }
}
