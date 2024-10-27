namespace SatisfactoryBot.Application.Domain.Admin.CheckOwnership;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Models.Dtos.Admin;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class CheckOwnershipQueryHandler : IRequestHandler<CheckOwnershipQuery, CheckOwnershipDto>
{
    #region Private Properties

    private readonly IDiscordServerRepository discordRepository;
    private readonly ILogger<CheckOwnershipQueryHandler> logger;

    #endregion Private Properties

    #region Public Constructor

    public CheckOwnershipQueryHandler(IDiscordServerRepository discordServerRepository,
        ILogger<CheckOwnershipQueryHandler> logger)
    {
        discordRepository = discordServerRepository;
        this.logger = logger;
    }

    #endregion Public Constructor

    public Task<CheckOwnershipDto> Handle(CheckOwnershipQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var activeSatisfactory = discordRepository.GetActiveSatisfactoryFromDiscordEntityId(request.GuildId);
            var result = new CheckOwnershipDto()
            {
                CheckOk = activeSatisfactory.Owner == request.ContextUserId,
                ServerName = activeSatisfactory.Name,
            };

            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking owner. {Ex}", ex.Message);
            throw;
        }
    }
}
