namespace SatisfactoryBot.Services.Api.Models;

using System.Text.Json.Serialization;

public class BaseResponse<T> : BaseBody<T> where T : new()
{
    [JsonPropertyName("errorCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ErrorMessage { get; set; }
}
