namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class SetAutoLoadSessionNameRequest
{
    [JsonPropertyName("SessionName")]
    public string SessionName { get; set; }
}
