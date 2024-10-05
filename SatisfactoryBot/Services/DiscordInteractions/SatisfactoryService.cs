namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord;
using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.EnumerateSessions;
using SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Application.Domain.GetOptions;
using SatisfactoryBot.Application.Domain.GetState;
using SatisfactoryBot.Application.Domain.ListServers;
using SatisfactoryBot.Application.Domain.RenameServer;
using SatisfactoryBot.Application.Domain.SetAutoLoadSessionName;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Services.Api.Models.Responses;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using SatisfactoryBot.Services.Api.Models;
using SatisfactoryBot.Application.Domain.UpdateActiveServer;

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

    [SlashCommand("autoload", "Updates the name of the session that the Dedicated Server will automatically load on startup")]
    public async Task SetAutoLoadSessionName() //[Summary(description: "Name of the session to automatically load on Dedicated Server startup")] string name
    {
        logger.LogInformation("SetAutoLoadSessionName command started");
        try
        {
            var result = await mediatr.Send(new EnumerateSessionsCommand(Context.Guild.Id));
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
            var result = await mediatr.Send(new EnumerateSessionsCommand(Context.Guild.Id));

            await RespondAsync(JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting available save game files: {Ex}", ex.Message);
            await RespondAsync("Error getting available save game files", ephemeral: true);
        }
    }

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

    [ComponentInteraction("autoload-session")]
    public async Task UpdateActiveServer(string[] selectedValues)
    {
        try
        {
            logger.LogInformation("updating autoload session {ServerId}", Context.Guild.Id);
            await DeferAsync();
            var result = await mediatr.Send(new SetAutoLoadSessionNameCommand(Context.Guild.Id, selectedValues[0]));
            await Context.Interaction.DeferAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing autoload session. {Ex}", ex.Message);
        }
    }
}
