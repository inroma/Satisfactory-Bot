namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Application.Domain.GetOptions;
using SatisfactoryBot.Application.Domain.GetState;
using SatisfactoryBot.Application.Domain.RenameServer;
using SatisfactoryBot.Application.Domain.RunCommand;
using SatisfactoryBot.Application.Domain.Shutdown;
using SatisfactoryBot.Helpers;
using System.Text.Json;
using System.Threading.Tasks;

public class SatisfactoryService : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<SatisfactoryService> logger;
    private readonly ISender mediatr;

    #endregion Private Properties

    #region Public Constructor

    public SatisfactoryService(ILogger<SatisfactoryService> logger, ISender sender)
    {
        this.logger = logger;
        mediatr = sender;
    }

    #endregion Public Constructor

    [SlashCommand("health", "Get server health")]
    public async Task Health()
    {
        logger.LogInformation("GetHealth command started");
        try
        {
            var result = await mediatr.Send(new GetHealthQuery(Context.Guild.Id));

            await RespondAsync(JsonSerializer.Serialize(result), ephemeral: true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting server Health: {Ex}", ex.Message);
            await RespondAsync("Error getting server Health", ephemeral: true);
        }
    }

    [SlashCommand("state", "Get server state")]
    public async Task ServerState()
    {
        logger.LogInformation("ServerState command started");
        try
        {
            var result = await mediatr.Send(new GetStateQuery(Context.Guild.Id));

            await RespondAsync(JsonSerializer.Serialize(result), ephemeral: true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting server State: {Ex}", ex.Message);
            await RespondAsync("Error getting server State", ephemeral: true);
        }
    }

    [SlashCommand("options", "Get server options")]
    public async Task ServerOptions()
    {
        logger.LogInformation("ServerOptions command started");
        try
        {
            var result = await mediatr.Send(new GetOptionsQuery(Context.Guild.Id));

            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting server Options: {Ex}", ex.Message);
            await RespondAsync("Error getting server Options", ephemeral: true);
        }
    }

    [SlashCommand("asettings", "Get advanced game settings")]
    public async Task AdvancedGameSettings()
    {
        logger.LogInformation("AdvancedGameSettings command started");
        try
        {
            var result = await mediatr.Send(new GetAdvancedGameSettingsQuery(Context.Guild.Id));

            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting advanced game settings: {Ex}", ex.Message);
            await RespondAsync("Error getting advanced game settings", ephemeral: true);
        }
    }

    [SlashCommand("rename", "Rename the Satisfactory Server")]
    public async Task RenameServer([Summary(description: "New server name to define")] string name)
    {
        logger.LogInformation("RenameServer command started");
        try
        {
            var result = await mediatr.Send(new RenameServerCommand(Context.Guild.Id, name));

            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting advanced game settings: {Ex}", ex.Message);
            await RespondAsync("Error getting advanced game settings", ephemeral: true);
        }
    }

    [SlashCommand("save", "Saves the current game")]
    public async Task RunServerCommand(string saveName)
    {
        try
        {
            logger.LogInformation("Start saving game from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new RunCommandCommand()
            {
                CommandName = "server.SaveGame",
                Value = saveName,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing server command server.SaveGame: {Ex}", ex.Message);
            await FollowupAsync($"Error executing server command server.SaveGame: {ex.Message}");
        }
    }

    [SlashCommand("auto-save", "Defines the Auto-save interval in seconds (default: 300)")]
    public async Task AutoSaveIntervalCommand([MinValue(0), MaxValue(7200)] int interval)
    {
        try
        {
            logger.LogInformation("Start updating Auto-save setting from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new RunCommandCommand()
            {
                CommandName = "FG.AutosaveInterval",
                Value = interval,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating Auto-save setting: {Ex}", ex.Message);
            await FollowupAsync($"Error updating Auto-save setting: {ex.Message}");
        }
    }

    [SlashCommand("event", "Enable/Disable in game FICSmas event")]
    public async Task DisableInGameEvent([Choice("Enabled", 1), Choice("Disabled", 0)] int status)
    {
        try
        {
            logger.LogInformation("Updating in-game event from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new RunCommandCommand()
            {
                CommandName = "FG.DisableSeasonalEvents",
                Value = status,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating in-game event: {Ex}", ex.Message);
            await FollowupAsync($"Error updating in-game event: {ex.Message}");
        }
    }

    [SlashCommand("network", "Changes the Network Quality Setting")]
    public async Task UpdateNetworkQualityCommand(
        [Choice("Low", 0), Choice("Medium", 1), Choice("High", 2), Choice("Ultra", 3)] int networkQuality)
    {
        try
        {
            logger.LogInformation("Start updating Network Quality from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new RunCommandCommand()
            {
                CommandName = "FG.NetworkQuality",
                Value = networkQuality,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating Network Quality: {Ex}", ex.Message);
            await FollowupAsync($"Error updating Network Quality: {ex.Message}");
        }
    }

    #region Shutdown

    [SlashCommand("shutdown", "Shutdown the Satisfactory server, use with caution")]
    public async Task ShutdownServer()
    {
        try
        {
            var buttons = ResponseHelper.CreateConfirmCancelButtons("shutdown");
            await RespondAsync("Use with caution, if your server isn't configured to restart on shutdown, it will stay offline.", components: buttons);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error prompting warning for server shutdown: {Ex}", ex.Message);
        }
    }

    [ComponentInteraction("shutdown-confirm")]
    public async Task ConfirmShutdownServer()
    {
        try
        {
            logger.LogInformation("Start shutting down server from User: {User}", Context.User.Id);
            var result = await mediatr.Send(new ShutdownCommand() { GuildId = Context.Guild.Id });
            await RespondAsync(result ? "Shutdown initiated" : "Shutdown failed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error shutting down server: {Ex}", ex.Message);
            await RespondAsync($"Error shutting down server: {ex.Message}");
        }
    }

    #endregion Shutdown
}
