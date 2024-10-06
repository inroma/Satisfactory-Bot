namespace SatisfactoryBot.Application.Domain.UpdateAdminPassword;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class UpdateAdminPasswordHandler : IRequestHandler<UpdateAdminPasswordCommand, bool>
{
    #region Private Properties

    private readonly ILogger<UpdateAdminPasswordHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public UpdateAdminPasswordHandler(ILogger<UpdateAdminPasswordHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(UpdateAdminPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                return await satisfactoryClient.UpdateAdminPassword(request.Password, server.Token);
                //TODO: Update all satis client in DB with new token ?
            }
            else
            {
                throw new Exception("No active Satisfactory server defined, please select one first with /list.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Server Admin password update: {Ex}", ex.Message);
            throw;
        }
    }
}
