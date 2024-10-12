namespace SatisfactoryBot.Services.Api.Models;

using System.Text.Json.Serialization;

public class BaseRequest<T> : BaseBody<T> where T : new()
{
    [JsonPropertyName("function")]
    public string Function { get; set; }

    #region Public Constructor

    public BaseRequest(string functionName)
    {
        Function = functionName;
    }

    #endregion Public Constructor
}
