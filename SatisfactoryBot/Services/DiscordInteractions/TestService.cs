namespace SatisfactoryBot.Services.DiscordInteractions;

using Discord.Interactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SatisfactoryBot.Application.Domain.GetHealth;
using SatisfactoryBot.Application.Domain.GetState;
using System.Text.Json;
using System.Threading.Tasks;

public class TestService : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<TestService> logger;
    private readonly ISender mediatr;

    #endregion Private Properties

    #region Public Constructor

    public TestService(ILogger<TestService> logger, ISender sender)
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
            await RespondAsync("Error getting server State", ephemeral: true);
        }
    }
}
