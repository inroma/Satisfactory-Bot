namespace SatisfactoryBot.Application.Domain.ListServers;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Services.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal class ListServersHandler : IRequestHandler<ListServersQuery, List<SatisfactoryServer>>
{
    #region Private Properties

    private readonly IDiscordServerRepository discordRepository;
    private readonly ILogger<ListServersHandler> logger;

    #endregion Private Properties

    #region Public Constructor

    public ListServersHandler(IDiscordServerRepository discordServerRepository,
        ILogger<ListServersHandler> logger)
    {
        discordRepository = discordServerRepository;
        this.logger = logger;
    }

    #endregion Public Constructor

    public Task<List<SatisfactoryServer>> Handle(ListServersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var servers = discordRepository.GetSatisfactoryServersListFromDiscordGuildId(request.GuildId);
            return Task.FromResult(servers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Discord's Satisfactory servers. {Ex}", ex.Message);
        }
        return null;
    }
}
