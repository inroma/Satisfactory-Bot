namespace SatisfactoryBot.Application.Domain.RenameServer;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class RenameServerHandler : IRequestHandler<RenameServerCommand, bool>
{
    #region Private Properties

    private readonly ILogger<RenameServerHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public RenameServerHandler(ILogger<RenameServerHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(RenameServerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetSatisfactoryServerFromDiscordGuildId(request.GuildId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                await satisfactoryClient.RenameServer(request.Name);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Server Rename: {Ex}", ex.Message);
            return false;
        }
        return false;
    }
}
