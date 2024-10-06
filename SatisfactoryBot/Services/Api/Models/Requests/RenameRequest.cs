namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class RenameRequest
{
    [JsonPropertyName("serverName")]
    public string ServerName { get; set; }
}
