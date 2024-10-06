namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Models.Dtos;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class GetStateHandler : IRequestHandler<GetStateQuery, ServerStateDto>
{
    private readonly ILogger<GetStateHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordRepository;

    public GetStateHandler(ILogger<GetStateHandler> logger, IDiscordServerRepository discordServerRepository)
    {
        this.logger = logger;
        discordRepository = discordServerRepository;
    }

    public async Task<ServerStateDto> Handle(GetStateQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server state");
        var server = discordRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);

        client = new SatisfactoryClient(server.Url, server.Token);

        var result = await client.GetState();
        return new()
        {
            ServerName = server.Name,
            StateResponse = result.Data
        };
    }
}
