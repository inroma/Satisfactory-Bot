namespace SatisfactoryBot.Services.Api.Models.Requests;

using SatisfactoryBot.Services.Api.Models.Misc;

public class PasswordLessLoginRequest()
{
    public string MinimumPrivilegeLevel { get; set; } = ApiPrivilegeLevel.Administrator;
}
