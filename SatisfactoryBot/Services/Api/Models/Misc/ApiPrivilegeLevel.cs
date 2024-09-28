namespace SatisfactoryBot.Services.Api.Models.Misc;

public readonly struct ApiPrivilegeLevel
{
    public static readonly string NotAuthenticated = "NotAuthenticated";
    public static readonly string Client = "Client";
    public static readonly string Administrator = "Administrator";
    public static readonly string InitialAdmin = "InitialAdmin";
    public static readonly string APIToken = "APIToken";
}
