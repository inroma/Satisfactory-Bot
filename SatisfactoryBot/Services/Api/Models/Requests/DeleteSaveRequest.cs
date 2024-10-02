namespace SatisfactoryBot.Services.Api.Models.Requests;

using System.Text.Json.Serialization;

internal class DeleteSaveRequest
{
    [JsonPropertyName("sessionName")]
    public string SessionName { get; set; }
}
