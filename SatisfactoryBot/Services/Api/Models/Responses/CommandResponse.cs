namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class CommandResponse
{
    [JsonPropertyName("commandResult")]
    public string CommandResult { get; set; }

    [JsonPropertyName("returnValue")]
    public bool ReturnValue { get; set; }
}
