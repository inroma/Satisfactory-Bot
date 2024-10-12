namespace SatisfactoryBot.Services.Api.Models.Responses;

using System.Text.Json.Serialization;

public class AdvancedGameSettingsResponse
{
    [JsonPropertyName("creativeModeEnabled")]
    public bool CreativeModeEnabled { get; set; }
    
    [JsonPropertyName("advancedGameSettings")]
    public Dictionary<string, string> AdvancedGameSettings { get; set; }
}
