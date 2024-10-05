namespace SatisfactoryBot.Application.Domain.UploadSaveGame;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

internal class UploadSaveGameCommandHandler : IRequestHandler<UploadSaveGameCommand, bool>
{
    #region Private Properties

    private readonly ILogger<UploadSaveGameCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public UploadSaveGameCommandHandler(ILogger<UploadSaveGameCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(UploadSaveGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            var discordApiClient = new RestClient();
            var fileData = await discordApiClient.DownloadDataAsync(new RestRequest(request.File.Url), cancellationToken);
            client = new SatisfactoryClient(server.Url, server.Token);
            await client.UploadSave(fileData, request.SaveName, request.LoadSaveGame, request.EnableAdvancedGameSettings);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading save file: {Ex}", ex.Message);
            throw;
        }
    }
}
