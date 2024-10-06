namespace SatisfactoryBot.Application.Domain.SaveGame;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class SaveGameCommandHandler : IRequestHandler<SaveGameCommand, bool>
{
    #region Private Properties

    private readonly ILogger<SaveGameCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public SaveGameCommandHandler(ILogger<SaveGameCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(SaveGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            client = new SatisfactoryClient(server.Url, server.Token);
            await client.SaveGame(request.SaveName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving game: {Ex}", ex.Message);
            throw;
        }
    }
}
