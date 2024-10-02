namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class SaveGameRequest
{
    [JsonPropertyName("saveName")]
    public string SaveName { get; set; }
}
