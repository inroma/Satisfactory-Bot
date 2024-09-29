namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.GetAdvancedGameSettings;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Application.Domain.GetOptions;
using SatisfactoryBot.Application.Domain.GetState;
using SatisfactoryBot.Application.Domain.RenameServer;
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
    public async Task Health(string url, string token)
    {
        logger.LogInformation("GetHealth command started");
        try
        {
            var result = await mediatr.Send(new GetHealthQuery(url, token));

            await RespondAsync(JsonSerializer.Serialize(result), ephemeral: true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting server Health: {Ex}", ex.Message);
            await RespondAsync("Error getting server Health", ephemeral: true);
        }
    }

    [SlashCommand("state", "Get server state")]
    public async Task ServerState(string url, string token)
    {
        logger.LogInformation("ServerState command started");
        try
        {
            var result = await mediatr.Send(new GetStateQuery(url, token));

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
}
