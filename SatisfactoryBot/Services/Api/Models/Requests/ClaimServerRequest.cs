namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class ClaimServerRequest
{
    [JsonPropertyName("serverName")]
    public string ServerName { get; set; }

    [JsonPropertyName("adminPassword")]
    public string AdminPassword { get; set; }
}
