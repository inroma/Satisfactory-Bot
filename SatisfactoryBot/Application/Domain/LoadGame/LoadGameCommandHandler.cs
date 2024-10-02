namespace SatisfactoryBot.Application.Domain.LoadGame;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class LoadGameCommandHandler : IRequestHandler<LoadGameCommand, bool>
{
    #region Private Properties

    private readonly ILogger<LoadGameCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public LoadGameCommandHandler(ILogger<LoadGameCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(LoadGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            client = new SatisfactoryClient(server.Url, server.Token);
            await client.LoadGame(request.SaveName, request.EnableAdvancedGameSettings);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading save: {Ex}", ex.Message);
            throw;
        }
    }
}
