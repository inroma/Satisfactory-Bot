namespace SatisfactoryBot.Services.Api.Models.Requests;

using SatisfactoryBot.Services.Api.Models.Misc;
using System.Text.Json.Serialization;

public class PasswordLessLoginRequest()
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApiPrivilegeLevel MinimumPrivilegeLevel { get; set; } = ApiPrivilegeLevel.Administrator;
}
