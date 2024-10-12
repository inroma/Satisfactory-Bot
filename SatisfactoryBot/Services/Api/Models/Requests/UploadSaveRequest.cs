namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

public class SaveGameFile
{
    [JsonPropertyName("SaveName")]
    public string SaveName { get; set; }

    [JsonPropertyName("loadSaveGame")]
    public bool LoadSaveGame { get; set; }

    [JsonPropertyName("enableAdvancedGameSettings")]
    public bool EnableAdvancedGameSettings { get; set; }
}