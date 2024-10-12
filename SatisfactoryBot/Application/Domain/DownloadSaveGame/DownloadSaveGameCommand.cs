namespace SatisfactoryBot.Application.Domain.DownloadSaveGame;

using MediatR;

internal class DownloadSaveGameCommand : IRequest<byte[]>
{
    public ulong EntityId { get; set; }

    public string SaveName { get; set; }
}
