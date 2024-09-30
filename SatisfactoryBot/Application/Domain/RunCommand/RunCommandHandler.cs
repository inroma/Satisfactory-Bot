namespace SatisfactoryBot.Application.Domain.RunCommand;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.RenameServer;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

internal class RunCommandHandler : IRequestHandler<RunCommandCommand, BaseResponse<CommandResponse>>
{
    #region Private Properties

    private readonly ILogger<RunCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public RunCommandHandler(ILogger<RunCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<BaseResponse<CommandResponse>> Handle(RunCommandCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            if (server != null)
            {
                client = new SatisfactoryClient(server.Url, server.Token);
                return await client.RunServerCommand(request.CommandName + (request.Value != null ? $" {request.Value}" : ""));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Ex}", ex.Message);
            throw;
        }
        return null;
    }
}
