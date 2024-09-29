namespace SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetAdvancedGameSettingsHandler : IRequestHandler<GetAdvancedGameSettingsQuery, BaseResponse<AdvancedGameSettingsResponse>>
{
    private ISatisfactoryClient client;
    private readonly ILogger<GetAdvancedGameSettingsHandler> logger;
    private readonly IDiscordServerRepository discordServerRepository;

    public GetAdvancedGameSettingsHandler(ILogger<GetAdvancedGameSettingsHandler> logger, IDiscordServerRepository repository)
    {
        this.logger = logger;
        discordServerRepository = repository;
    }


    public async Task<BaseResponse<AdvancedGameSettingsResponse>> Handle(GetAdvancedGameSettingsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server health");

        var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);

        client = new SatisfactoryClient(server.Url, server.Token);

        return await client.GetAdvancedGameSettings();
    }
}
