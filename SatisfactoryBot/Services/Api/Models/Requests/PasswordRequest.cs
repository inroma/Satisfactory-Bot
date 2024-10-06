namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class PasswordRequest
{
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
