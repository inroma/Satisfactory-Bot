namespace SatisfactoryBot.Application.Domain.GetHealth;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Models.Dtos;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetHealthHandler : IRequestHandler<GetHealthQuery, ServerHealthDto>
{
    private readonly ILogger<GetHealthHandler> logger;
    private readonly IDiscordServerRepository repository;

    public GetHealthHandler(ILogger<GetHealthHandler> logger, IDiscordServerRepository discordServerRepository)
    {
        this.logger = logger;
        repository = discordServerRepository;
    }

    public async Task<ServerHealthDto> Handle(GetHealthQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server health");

        var server = repository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);

        var client = new SatisfactoryClient(server.Url, server.Token);

        var result = await client.GetHealth();

        return new ServerHealthDto()
        {
            HealthResponse = result.Data,
            ServerName = server.Name
        };
    }
}
