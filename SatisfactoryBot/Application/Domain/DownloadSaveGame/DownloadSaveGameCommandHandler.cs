namespace SatisfactoryBot.Application.Domain.DownloadSaveGame;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.DeleteSave;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class DownloadSaveGameCommandHandler : IRequestHandler<DownloadSaveGameCommand, byte[]>
{
    #region Private Properties

    private readonly ILogger<DownloadSaveGameCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public DownloadSaveGameCommandHandler(ILogger<DownloadSaveGameCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<byte[]> Handle(DownloadSaveGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            client = new SatisfactoryClient(server.Url, server.Token);
            return await client.DownloadSave(request.SaveName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading save file: {Ex}", ex.Message);
            throw;
        }
    }
}
