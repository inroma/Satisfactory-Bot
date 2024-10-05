namespace SatisfactoryBot.Application.Domain.UploadSaveGame;

using Discord;
using MediatR;

internal class UploadSaveGameCommand : IRequest<bool>
{
    public ulong GuildId { get; set; }

    public string SaveName { get; set; }

    public bool LoadSaveGame { get; set; }

    public bool EnableAdvancedGameSettings { get; set; }

    public IAttachment File { get; set; }
}
