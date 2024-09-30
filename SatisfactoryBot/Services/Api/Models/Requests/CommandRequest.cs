namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class CommandRequest
{
    [JsonPropertyName("command")]
    public string Command { get; set; }
}
