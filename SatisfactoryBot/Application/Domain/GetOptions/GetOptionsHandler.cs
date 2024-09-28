namespace SatisfactoryBot.Application.Domain.GetOptions;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class GetOptionsHandler : IRequestHandler<GetOptionsQuery, BaseResponse<OptionsResponse>>
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

    public async Task<BaseResponse<OptionsResponse>> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving server options");

        var server = discordRepository.GetSatisfactoryServerFromDiscordGuildId(request.GuildId);

        client = new SatisfactoryClient(server.Url, server.Token);

        return await client.GetOptions();
    }
}
