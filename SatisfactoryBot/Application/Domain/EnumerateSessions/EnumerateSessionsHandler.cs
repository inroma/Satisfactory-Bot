namespace SatisfactoryBot.Application.Domain.EnumerateSessions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api;
using System;
using System.Threading.Tasks;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;

internal class EnumerateSessionsHandler : IRequestHandler<EnumerateSessionsCommand, BaseResponse<EnumerateSessionsResponse>>
{
    #region Private Properties

    private readonly ILogger<EnumerateSessionsHandler> logger;
    private ISatisfactoryClient satisfactoryClient;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public EnumerateSessionsHandler(ILogger<EnumerateSessionsHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<BaseResponse<EnumerateSessionsResponse>> Handle(EnumerateSessionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            if (server != null)
            {
                satisfactoryClient = new SatisfactoryClient(server.Url, server.Token);
                return await satisfactoryClient.GetSessions();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Enumerate Sessions: {Ex}", ex.Message);
            return null;
        }
        return null;
    }
}