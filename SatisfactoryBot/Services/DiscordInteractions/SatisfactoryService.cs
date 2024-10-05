namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SatisfactoryBot.Application.Domain.DeleteSave;
using SatisfactoryBot.Application.Domain.DeleteSaveSession;
using SatisfactoryBot.Application.Domain.DownloadSaveGame;
using SatisfactoryBot.Application.Domain.EnumerateSessions;
using SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Application.Domain.GetOptions;
using SatisfactoryBot.Application.Domain.GetState;
using SatisfactoryBot.Application.Domain.LoadGame;
using SatisfactoryBot.Application.Domain.NewGame;
using SatisfactoryBot.Application.Domain.RenameServer;
using SatisfactoryBot.Application.Domain.RunCommand;
using SatisfactoryBot.Application.Domain.SaveGame;
using SatisfactoryBot.Application.Domain.Shutdown;
using SatisfactoryBot.Application.Domain.UploadSaveGame;
using SatisfactoryBot.Helpers;
using SatisfactoryBot.Models.Settings;
using SatisfactoryBot.Services.DiscordInteractions.Attributes;
using SatisfactoryBot.Application.Domain.SetAutoLoadSessionName;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using SatisfactoryBot.Services.Api.Models;

public class SatisfactoryService : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<SatisfactoryService> logger;
    private readonly ISender mediatr;
    private readonly GlobalSettings globalSettings;

    #endregion Private Properties

    #region Public Constructor

    public SatisfactoryService(ILogger<SatisfactoryService> logger, ISender sender, IOptions<GlobalSettings> options)
    {
        this.logger = logger;
        mediatr = sender;
        globalSettings = options.Value;
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
            var buttons = ResponseHelper.CreateConfirmCancelButtons("shutdown", Context.User.Id);
            await RespondAsync("Use with caution, if your server isn't configured to restart on shutdown, it will stay offline.", components: buttons);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error prompting warning for server shutdown: {Ex}", ex.Message);
        }
    }

    [DoUserCheck]
    [ComponentInteraction("shutdown-confirm:*")]
    public async Task ConfirmShutdownServer()
    {
        try
        {
            logger.LogInformation("Start shutting down server from User: {User}", Context.User.Id);
            var result = await mediatr.Send(new ShutdownCommand() { GuildId = Context.Guild.Id });
            await (Context.Interaction as SocketMessageComponent).Message.ModifyAsync((a) =>
            {
                a.Content = result ? "Shutdown initiated" : "Shutdown failed";
                a.Components = null;
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error shutting down server: {Ex}", ex.Message);
            await RespondAsync($"Error shutting down server: {ex.Message}");
        }
    }

    #endregion Shutdown

    [SlashCommand("new-game", "Creates a new game")]
    public async Task CreateNewGame(
        [Summary("Session-Name"), MinLength(3), MaxLength(80)] string sessionName,
        [Summary("Start-Location"), Choice("⛺ Grass Fields", "Grass Fields"),
            Choice("🗿 Rocky Desert", "Rocky Desert"),
            Choice("🌲 Northern Forest", "Northern Forest"),
            Choice("🌵 Dune Desert", "Dune Desert")] string startLocation,
        [Summary("Skip-Onboarding")] bool skipOnboarding = true)
    {
        try
        {
            logger.LogInformation("Game creation started by {UserId}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new NewGameCommand()
            {
                SessionName = sessionName,
                StartLocation = startLocation,
                SkipOnboarding = skipOnboarding,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(result ? "Game created succesfuly! Wait 2 minutes for the session to start." : "Game creation failed 😕");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Create new game error: {Ex}", e.Message);
        }
    }

    #region Save

    [SlashCommand("save", "Saves the current game")]
    public async Task RunCommandSave(string saveName)
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

    [SlashCommand("delete-save-file", "Deletes the specified save file")]
    public async Task DeleteSaveFile([Summary(description: "save filename to delete")] string fileName)
    {
        try
        {
            logger.LogInformation("Start deleting save file from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new DeleteSaveFileCommand()
            {
                SaveName = fileName,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(result ? "Save file deleted successfully" : "Failed to delete save file");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting save file: {Ex}", ex.Message);
            await FollowupAsync($"Error deleting save file: {ex.Message}");
        }
    }

    [SlashCommand("delete-save-session", "Deletes all save files belonging to the specific session name")]
    public async Task DeleteSaveSession([Summary(description: "session name to delete")] string session)
    {
        try
        {
            logger.LogInformation("Start deleting session from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new DeleteSaveSessionCommand()
            {
                SessionName = session,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(result ? "Session deleted successfully" : "Failed to delete Session");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting session: {Ex}", ex.Message);
            await FollowupAsync($"Error deleting session: {ex.Message}");
        }
    }

    [SlashCommand("new-save", "Creates a new save file of the current game")]
    public async Task SaveGame(string saveName)
    {
        try
        {
            logger.LogInformation("Start creating new save from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new SaveGameCommand()
            {
                SaveName = saveName,
                GuildId = Context.Guild.Id
            });
            await FollowupAsync(result ? "Save file created successfully" : "Failed to create a Save file 😕");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating new save: {Ex}", ex.Message);
            await FollowupAsync($"Error: {ex.Message}");
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

    #endregion Save

    [SlashCommand("load", "Loads the save game file by name")]
    public async Task LoadGame(
        [Summary(description: "Name of the save game file to load"), MinLength(3)] string saveName,
        [Summary(description: "True if save game file should be loaded with Advanced Game Settings enabled")] bool advancedSettings = false)
    {
        logger.LogInformation("Start loading game from User: {User}", Context.User.Id);
        try
        {
            var result = await mediatr.Send(new LoadGameCommand()
            {
                GuildId = Context.Guild.Id,
                SaveName = saveName,
                EnableAdvancedGameSettings = advancedSettings
            });

            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading game: {Ex}", ex.Message);
            await RespondAsync("Error loading game", ephemeral: true);
        }
    }

    [SlashCommand("download", "Downloads save game with the given name from the Dedicated Server.")]
    public async Task DownloadSaveFile(
        [Summary(description: "Name of the save game file to download from the Dedicated Server")] string fileName)
    {
        try
        {
            logger.LogInformation("Downloading save from User: {User}", Context.User.Id);
            await DeferAsync();
            var result = await mediatr.Send(new DownloadSaveGameCommand()
            {
                GuildId = Context.Guild.Id,
                SaveName = fileName,
            });
            var stream = new MemoryStream();
            await stream.WriteAsync(result);
            await FollowupWithFileAsync(stream, fileName);
            await stream.DisposeAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading save: {Ex}", ex.Message);
            await FollowupAsync($"Error downloading save: {ex.Message}");
        }
    }

    [SlashCommand("upload", "Uploads save game file to the Dedicated Server with the given name.")]
    public async Task UploadSaveFile(
        IAttachment file,
        [Summary(description: "Name of the save game file to create on the Dedicated Server")] string saveName,
        [Summary(description: "True if save game file should be immediately loaded by the Dedicated Server")] bool load,
        [Summary(description: "True if save game file should be loaded with Advanced Game Settings enabled")] bool advancedSettings)
    {
        try
        {
            logger.LogInformation("Uploading save from User: {User}", Context.User.Id);
            await DeferAsync();
            if (file.Size > globalSettings.DiscordSettings.MaxFileSize)
            {
                await FollowupAsync($"Process canceled, max file size is {globalSettings.DiscordSettings.MaxFileSize/1000000} Mo.");
                return;
            }
            var result = await mediatr.Send(new UploadSaveGameCommand()
            {
                GuildId = Context.Guild.Id,
                SaveName = saveName ?? file.Filename,
                LoadSaveGame = load,
                EnableAdvancedGameSettings = advancedSettings,
                File = file
            });
            var resultStringSuccess = $"Save uploaded {(advancedSettings ? "with advanced settings" : "")}{(load ? "and is being loaded by the server" : "")}!";
            await FollowupAsync(result ? resultStringSuccess : "File upload failed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading save: {Ex}", ex.Message);
            await FollowupAsync($"Error uploading save: {ex.Message}");
        }
    }

    [SlashCommand("autoload", "Updates the name of the session that the Dedicated Server will automatically load on startup")]
    public async Task SetAutoLoadSessionName()
    {
        logger.LogInformation("SetAutoLoadSessionName command started");
        try
        {
            var result = await mediatr.Send(new EnumerateSessionsQuery(Context.Guild.Id));
            var menu = CreateSelectMenu(result);
            var builder = new ComponentBuilder().WithSelectMenu(menu);
            await RespondAsync("Satisfactory auto-load session", components: builder.Build(), ephemeral: true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting autoload session: {Ex}", ex.Message);
            await RespondAsync("Error setting autoload session", ephemeral: true);
        }
    }

    [SlashCommand("sessions", "Enumerates all save game files available on the Dedicated Server")]
    public async Task EnumerateSessions()
    {
        logger.LogInformation("EnumerateSessions command started");
        try
        {
            var result = await mediatr.Send(new EnumerateSessionsQuery(Context.Guild.Id));
            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting available save game files: {Ex}", ex.Message);
            await RespondAsync("Error getting available save game files", ephemeral: true);
        }
    }

    [ComponentInteraction("autoload-session")]
    public async Task UpdateActiveServer(string[] selectedValues)
    {
        try
        {
            logger.LogInformation("updating autoload session {ServerId}", Context.Guild.Id);
            await DeferAsync();
            var result = await mediatr.Send(new SetAutoLoadSessionNameCommand(Context.Guild.Id, selectedValues[0]));
            await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing autoload session. {Ex}", ex.Message);
        }
    }

    #region Private Methods

    private static SelectMenuBuilder CreateSelectMenu(BaseResponse<EnumerateSessionsResponse> response)
    {
        var menuBuilder = new SelectMenuBuilder()
        {
            CustomId = "autoload-session",
            MaxValues = 1,
            MinValues = 1
        };
        foreach (var (value, i) in response.Data.Sessions.Select((Value, i) => (Value, i)))
        {
            menuBuilder.AddOption(new()
            {
                Label = value.SessionName,
                Description = value.SaveHeaders.LastOrDefault().SaveName,
                Value = value.SessionName,
                IsDefault = (i == response.Data.CurrentSessionIndex)
            });
        };
        return menuBuilder;
    }

    #endregion Private Methods
}
