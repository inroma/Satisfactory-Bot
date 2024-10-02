namespace SatisfactoryBot.Application.Domain.DeleteSave;

using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Services.Api;
using SatisfactoryBot.Services.Api.Interfaces;
using System.Threading;
using System.Threading.Tasks;

internal class DeleteSaveFileCommandHandler : IRequestHandler<DeleteSaveFileCommand, bool>
{
    #region Private Properties

    private readonly ILogger<DeleteSaveFileCommandHandler> logger;
    private ISatisfactoryClient client;
    private readonly IDiscordServerRepository discordServerRepository;

    #endregion Private Properties

    #region Public Constructor

    public DeleteSaveFileCommandHandler(ILogger<DeleteSaveFileCommandHandler> logger, IDiscordServerRepository serverRepository)
    {
        this.logger = logger;
        discordServerRepository = serverRepository;
    }

    #endregion Public Constructor

    public async Task<bool> Handle(DeleteSaveFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var server = discordServerRepository.GetActiveSatisfactoryFromDiscordGuildId(request.GuildId);
            client = new SatisfactoryClient(server.Url, server.Token);
            var result = await client.DeleteSaveFile(request.SaveName);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting save file: {Ex}", ex.Message);
            throw;
        }
    }
}
