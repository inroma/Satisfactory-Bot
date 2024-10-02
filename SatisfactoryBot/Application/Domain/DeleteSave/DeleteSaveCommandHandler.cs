namespace SatisfactoryBot.Application.Domain.DeleteSave;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class DeleteSaveCommandHandler : IRequestHandler<DeleteSaveCommand, bool>
{
    #region Private Properties

    private readonly ILogger<DeleteSaveCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public DeleteSaveCommandHandler(ILogger<DeleteSaveCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(DeleteSaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            client = new SatisfactoryClient(server.Url, server.Token);
            var result = await client.DeleteSave(request.SaveName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting save file: {Ex}", ex.Message);
            throw;
        }
    }
}
