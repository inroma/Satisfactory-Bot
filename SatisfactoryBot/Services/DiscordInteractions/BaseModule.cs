namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Services.DiscordInteractions.Attributes;

public class BaseModule : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<BaseModule> logger;

    #endregion Private Properties

    #region Public Constructor

    public BaseModule(ILogger<BaseModule> logger)
    {
        this.logger = logger;
    }

    #endregion Public Constructor

    #region Public Methods

    [ComponentInteraction("delete-interaction")]
    [DoUserCheck]
    public async Task DeleteInteraction()
    {
        try
        {
            await Context.Channel.DeleteMessageAsync(((SocketMessageComponent)Context.Interaction).Message.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting message: {Ex}", ex.Message);
        }
        await Task.CompletedTask;
    }

    #endregion Public Methods

}
