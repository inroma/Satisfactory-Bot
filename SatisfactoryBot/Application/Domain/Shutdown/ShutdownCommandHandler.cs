namespace SatisfactoryBot.Application.Domain.Shutdown;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class ShutdownCommandHandler : IRequestHandler<ShutdownCommand, bool>
{
    #region Private Properties

    private readonly ILogger<ShutdownCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public ShutdownCommandHandler(ILogger<ShutdownCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(ShutdownCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            client = new SatisfactoryClient(server.Url, server.Token);
            await client.Shutdown();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error shutting down server: {Ex}", ex.Message);
            throw;
        }
    }
}
