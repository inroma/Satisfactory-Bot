namespace SatisfactoryBot.Application.Domain.DeleteSaveSession;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api.Interfaces;
using SatisfactoryBot.Services.Api;
using System.Threading;
using System.Threading.Tasks;

internal class DeleteSaveSessionCommandHandler : IRequestHandler<DeleteSaveSessionCommand, bool>
{
    #region Private Properties

    private readonly ILogger<DeleteSaveSessionCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public DeleteSaveSessionCommandHandler(ILogger<DeleteSaveSessionCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(DeleteSaveSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            client = new SatisfactoryClient(server.Url, server.Token);
            await client.DeleteSessionSave(request.SessionName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting save session: {Ex}", ex.Message);
            throw;
        }
    }
}
