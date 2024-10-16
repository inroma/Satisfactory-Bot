﻿namespace SatisfactoryBot.Application.Domain.GetOptions;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Models.Dtos;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class GetOptionsHandler : IRequestHandler<GetOptionsQuery, ServerOptionsDto>
{
    #region Private Properties
    
    private readonly ILogger<GetOptionsHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordRepository;

    #endregion Private Properties

    #region Public Constructor

    public GetOptionsHandler(ILogger<GetOptionsHandler> logger, IDiscordServerRepository discordServerRepository)
    {
        this.logger = logger;
        discordRepository = discordServerRepository;
    }

    #endregion Public Constructor

    public async Task<ServerOptionsDto> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server options");

        var server = discordRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);

        client = new SatisfactoryClient(server.Url, server.Token);

        var result = await client.GetOptions();
        return new()
        {
            ServerName = server.Name,
            OptionsResponse = result.Data
        };
    }
}
