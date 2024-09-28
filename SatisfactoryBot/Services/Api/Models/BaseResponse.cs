namespace SatisfactoryBot.Services.Api.Models;

using System.Text.Json.Serialization;

public class BaseResponse<T> : BaseBody<T> where T : new()
{
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; }
}
