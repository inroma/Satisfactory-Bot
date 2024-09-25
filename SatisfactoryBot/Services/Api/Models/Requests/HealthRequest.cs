namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class HealthRequest
{
    [JsonPropertyName("clientCustomData")]
    public object ClientCustomData { get; set; }
}
