namespace SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Models.Dtos;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class GetAdvancedGameSettingsHandler : IRequestHandler<GetAdvancedGameSettingsQuery, ServerAdvancedSettingsDto>
{
    private ISatisfactoryClient client;
    private readonly ILogger<GetAdvancedGameSettingsHandler> logger;
    private readonly IDiscordServerRepository discordServerRepository;

    public GetAdvancedGameSettingsHandler(ILogger<GetAdvancedGameSettingsHandler> logger, IDiscordServerRepository repository)
    {
        this.logger = logger;
        discordServerRepository = repository;
    }


    public async Task<ServerAdvancedSettingsDto> Handle(GetAdvancedGameSettingsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server health");

        var server = discordServerRepository.GetActiveSatisfactoryFromDiscordEntityId(request.EntityId);

        client = new SatisfactoryClient(server.Url, server.Token);

        var result = await client.GetAdvancedGameSettings();
        return new()
        {
            ServerName = server.Name,
            AdvancedGameSettings = result.Data
        };
    }
}
