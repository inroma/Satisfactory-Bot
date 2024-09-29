namespace SatisfactoryBot.Application.Domain.GetState;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetStateHandler : IRequestHandler<GetStateQuery, BaseResponse<StateResponse>>
{
    private readonly ILogger<GetStateHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordRepository;

    public GetStateHandler(ILogger<GetStateHandler> logger, IDiscordServerRepository discordServerRepository)
    {
        this.logger = logger;
        discordRepository = discordServerRepository;
    }

    public async Task<BaseResponse<StateResponse>> Handle(GetStateQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server state");
        var server = discordRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);

        client = new SatisfactoryClient(server.Url, server.Token);

        return await client.GetState();
    }
}
