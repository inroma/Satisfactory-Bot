namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class OptionsResponse
{
    [JsonPropertyName("serverOptions")]
    public Dictionary<string, string> ServerOptions { get; set; }

    [JsonPropertyName("pendingServerOptions")]
    public Dictionary<string, string> PendingServerOptions { get; set; }
}
