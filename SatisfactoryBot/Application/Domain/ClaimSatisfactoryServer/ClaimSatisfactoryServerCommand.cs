namespace SatisfactoryBot.Application.Domain.ClaimSatisfactoryServer;

using MediatR;

public record ClaimSatisfactoryServerCommand : IRequest<bool>
{
    public string Url { get; set; }

    public string Password { get; set; }

    public string ServerName { get; set; }

    public string Token { get; set; }

    public ulong EntityId { get; set; }

    public ulong UserId { get; set; }
}
