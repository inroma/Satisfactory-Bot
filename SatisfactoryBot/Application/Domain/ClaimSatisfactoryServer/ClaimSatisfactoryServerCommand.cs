namespace SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;

using MediatR;

public record ClaimSatisfactoryServerCommand : IRequest<bool>
{
    public string Url { get; set; }

    public string Password { get; set; }

    public string Token { get; set; }

    public ulong GuildId { get; set; }

    public ulong UserId { get; set; }

    public ClaimSatisfactoryServerCommand(string url, string password, string token, ulong guildId, ulong userId)
    {
        Url = url;
        Password = password;
        Token = token;
        GuildId = guildId;
        UserId = userId;
    }
}
