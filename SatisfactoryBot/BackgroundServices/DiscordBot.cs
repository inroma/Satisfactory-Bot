namespace SatisfactoryBot.BackgroundServices;

using Discord.Interactions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SatisfactoryBot.Models.Settings;

public class DiscordBot : BackgroundService
{
    #region Private Fields.

    readonly IServiceProvider serviceProvider;
    readonly GlobalSettings globalConfiguration = new();
    readonly DiscordSocketClient client = null;
    readonly ILogger<DiscordBot> logger;

    private bool isFirstTimeBooting = true;

    #endregion Private Fields

    #region Public Constructor

    public DiscordBot(IServiceProvider services)
    {
        serviceProvider = services.CreateScope().ServiceProvider;
        client = serviceProvider.GetRequiredService<DiscordSocketClient>();
        globalConfiguration = serviceProvider.GetRequiredService<IOptions<GlobalSettings>>().Value;
        logger = serviceProvider.GetRequiredService<ILogger<DiscordBot>>();
        client.Log += LogAsync;
    }

    #endregion Public Constructor

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // this is where we get the Token value from the configuration file, and start the bot
        await client.LoginAsync(TokenType.Bot, globalConfiguration.DiscordSettings.Token);
        await client.StartAsync();
        client.Ready += async () => {
            if (isFirstTimeBooting)
            {
                var interactionService = new InteractionService(client.Rest);
                await interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), serviceProvider);
                if (globalConfiguration.DiscordSettings.RegisterCommandsGlobally)
                {
                    await interactionService.RegisterCommandsGloballyAsync();
                    logger.LogInformation("Commands registered globally");
                }
                else if (globalConfiguration.DiscordSettings.DevServerId.HasValue)
                {
                    await interactionService.RegisterCommandsToGuildAsync(globalConfiguration.DiscordSettings.DevServerId.Value);
                    logger.LogInformation("Commands registered for guild {guildId}", globalConfiguration.DiscordSettings.DevServerId.Value);
                }
                client.InteractionCreated += async interaction =>
                {
                    var scope = serviceProvider.CreateScope();
                    var ctx = new SocketInteractionContext(client, interaction);
                    await interactionService.ExecuteCommandAsync(ctx, scope?.ServiceProvider);
                };
                isFirstTimeBooting = false;
            }

            await Task.CompletedTask;
        };
    }

    private Task LogAsync(LogMessage message)
    {
        var loglevel = message.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };

        logger.Log(loglevel, message.Exception?.Message ?? message.Message);

        return Task.CompletedTask;
    }
}
