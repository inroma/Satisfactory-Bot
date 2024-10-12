namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class AuthResponse
{
    [JsonPropertyName("authenticationToken")]
    public string AuthenticationToken { get; set; }
}
