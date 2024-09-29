namespace SatisfactoryBot.Application.Domain.UpdateClientPassword;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class UpdateClientPasswordHandler : IRequestHandler<UpdateClientPasswordCommand, bool>
{
    #region Private Properties

    private readonly ILogger<UpdateClientPasswordHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public UpdateClientPasswordHandler(ILogger<UpdateClientPasswordHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(UpdateClientPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                return await satisfactoryClient.UpdateClientPassword(request.Password);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Server Client password update: {Ex}", ex.Message);
            return false;
        }
        return false;
    }
}
