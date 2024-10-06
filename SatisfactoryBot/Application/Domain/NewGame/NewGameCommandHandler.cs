namespace SatisfactoryBot.Application.Domain.NewGame;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api;
using System.Threading;
using System.Threading.Tasks;

public class NewGameCommandHandler : IRequestHandler<NewGameCommand, bool>
{
    #region Private Properties

    private readonly ILogger<NewGameCommandHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public NewGameCommandHandler(ILogger<NewGameCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(NewGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                await satisfactoryClient.CreateNewGame(request.SessionName, request.StartLocation, request.SkipOnboarding);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during game creation: {Ex}", ex.Message);
            return false;
        }
        return false;
    }
}
