namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class AdminPasswordRequest
{
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("authenticationToken")]
    public string AuthenticationToken { get; set; }
}
