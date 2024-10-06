namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class CreateNewGameRequest
{
    [JsonPropertyName("NewGameData")]
    public NewGameData NewGameData { get; set; }
}

internal class NewGameData
{
    [JsonPropertyName("SessionName")]
    public string SessionName { get; set; }

    [JsonPropertyName("MapName")]
    public string MapName { get; set; } = null;

    [JsonPropertyName("StartingLocation")]
    public string StartingLocation { get; set; }

    // Currently SkipOnboarding must be set to bSkipOnboarding, due to an issue with the property names.
    [JsonPropertyName("bSkipOnboarding")]
    public bool SkipOnboarding { get; set; }

    [JsonPropertyName("AdvancedGameSettings")]
    public Dictionary<string, string> AdvancedGameSettings { get; set; }

    [JsonPropertyName("CustomOptionsOnlyForModding")]
    public Dictionary<string, string> CustomOptionsOnlyForModding { get; set; }
}
