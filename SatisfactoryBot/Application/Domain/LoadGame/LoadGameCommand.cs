namespace SatisfactoryBot.Application.Domain.LoadGame;

using MediatR;

internal class LoadGameCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SaveName { get; set; }

    public bool EnableAdvancedGameSettings { get; set; }
}
