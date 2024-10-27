namespace SatisfactoryBot.Application.Domain.Admin.TransferOwnership;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Data;
using SatisfactoryBot.Services.Api.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class TransferOwnershipCommandHandler : IRequestHandler<TransferOwnershipCommand, bool>
{
    #region Private Properties

    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
    private readonly IDiscordServerRepository discordRepository;
    private readonly ILogger<TransferOwnershipCommandHandler> logger;

    #endregion Private Properties

    #region Public Constructor

    public TransferOwnershipCommandHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IDiscordServerRepository discordServerRepository,
        ILogger<TransferOwnershipCommandHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        discordRepository = discordServerRepository;
        this.logger = logger;
    }

    #endregion Public Constructor

    public Task<bool> Handle(TransferOwnershipCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordRepository.GetActiveSatisfactoryFromDiscordEntityId(request.GuildId);
            server.Owner = request.NewOwnerId;
            var result = unitOfWork.Save();
            return Task.FromResult(result > 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error switching owner. {Ex}", ex.Message);
            throw;
        }
    }
}
