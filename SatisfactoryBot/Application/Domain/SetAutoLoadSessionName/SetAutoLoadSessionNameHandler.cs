namespace SatisfactoryBot.Application.Domain.SetAutoLoadSessionName;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.RenameServer;
using SatisfactoryBot.Data.Repositories;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class SetAutoLoadSessionNameHandler : IRequestHandler<SetAutoLoadSessionNameCommand, bool>
{
    #region Private Properties
    private readonly ILogger<SetAutoLoadSessionNameHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;
    #endregion Private Properties

    #region Public Constructor

    public SetAutoLoadSessionNameHandler(ILogger<SetAutoLoadSessionNameHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(SetAutoLoadSessionNameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                await satisfactoryClient.UpdateAutoLoadSessionName(request.SessionName);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting the server autoload session name: {Ex}", ex.Message);
            return false;
        }
        return false;
    }
}