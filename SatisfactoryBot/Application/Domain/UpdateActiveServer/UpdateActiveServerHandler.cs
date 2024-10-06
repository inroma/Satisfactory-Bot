namespace SatisfactoryBot.Application.Domain.UpdateActiveServer;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using SatisfactoryBot.Data.Repositories.Interfaces;

internal class UpdateActiveServerHandler : IRequestHandler<UpdateActiveServerCommand, bool>
{
    #region Private Properties

    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
    private readonly IDiscordServerRepository discordRepository;
    private readonly ILogger<UpdateActiveServerHandler> logger;

    #endregion Private Properties

    #region Public Constructor

    public UpdateActiveServerHandler(IUnitOfWork<ApplicationDbContext> unitOfWork,
        IDiscordServerRepository discordServerRepository,
        ILogger<UpdateActiveServerHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        discordRepository = discordServerRepository;
        this.logger = logger;
    }

    #endregion Public Constructor

    public Task<bool> Handle(UpdateActiveServerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var servers = discordRepository.GetSatisfactoryServersListFromDiscordEntityId(request.EntityId);
            servers.Where(s => s.Id != request.NewlyActiveServerId).ToList().ForEach(s =>
            {
                s.IsDefaultServer = false;
            });
            servers.First(s => s.Id == request.NewlyActiveServerId).IsDefaultServer = true;
            return Task.FromResult(unitOfWork.Save() > 0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating entities: {Ex}", ex.Message);
        }
        return Task.FromResult(false);
    }
}
