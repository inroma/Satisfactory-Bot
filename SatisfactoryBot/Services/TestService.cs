namespace SatisfactoryBot.Services;

using Discord.Interactions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class TestService : InteractionModuleBase<SocketInteractionContext>
{
    #region Private Properties

    private readonly ILogger<TestService> logger;

    #endregion Private Properties

    #region Public Constructor

    public TestService(ILogger<TestService> logger)
    {
        this.logger = logger;
    }

    #endregion Public Constructor

    [SlashCommand("echo", "Repeat the input")]
    public async Task Echo(string echo, [Summary(description: "mention the user")] bool mention = false)
    {
        logger.LogInformation("echo command received");
        await RespondAsync(echo + (mention ? Context.User.Mention : string.Empty));
        logger.LogInformation("echo command responded");
    }

    [SlashCommand("ping", "Pings the bot and returns its latency.")]
    public async Task GreetUserAsync()
        => await RespondAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!", ephemeral: true);
}
