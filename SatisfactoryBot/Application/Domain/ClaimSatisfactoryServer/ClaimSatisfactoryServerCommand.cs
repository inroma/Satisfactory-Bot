namespace SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;

using MediatR;

public record ClaimSatisfactoryServerCommand : IRequest<bool>
{
    public string Url { get; set; }

    public string Token { get; set; }

    public ulong GuildId { get; set; }

    public ulong UserId { get; set; }

    public ClaimSatisfactoryServerCommand(string url, string token, ulong guildId, ulong userId)
    {
        Url = url;
        Token = token;
        GuildId = guildId;
        UserId = userId;
    }
}
