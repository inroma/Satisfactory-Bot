namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class LoadGameRequest
{
    [JsonPropertyName("saveName")]
    public string SaveName { get; set; }

    [JsonPropertyName("enableAdvancedGameSettings")]
    public bool EnableAdvancedGameSettings { get; set; }
}
