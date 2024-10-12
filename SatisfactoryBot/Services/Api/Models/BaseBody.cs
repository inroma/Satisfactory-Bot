namespace SatisfactoryBot.Services.Api.Models;

using System.Text.Json.Serialization;

public class BaseBody<T> where T : new()
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    public BaseBody()
    {
        Data = new T();
    }
}
