namespace SatisfactoryBot;

using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatisfactoryBot.Data;
using SatisfactoryBot.BackgroundServices;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SatisfactoryBot.Data.UnitOfWork;
using SatisfactoryBot.Models.Settings;

internal class Program
{
    static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args).ConfigureServices((ctxc, services) => {
            var configuration = MapSettings(ctxc);
            ConfigureServices(services, configuration);
        }).Build().Run();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        var config = new DiscordSocketConfig
        {
            //AlwaysDownloadUsers = true,
            GatewayIntents =
                GatewayIntents.Guilds |
                //GatewayIntents.GuildMembers |
                GatewayIntents.DirectMessages
        };

        services.Configure<GlobalSettings>(configuration)
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Database")).UseLazyLoadingProxies())
            .AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>()
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>()
            .AddHostedService<DiscordBot>()
            .AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog("files/nlog.config");
            })
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }

    private static IConfigurationRoot MapSettings(HostBuilderContext context)
    {
        GlobalSettings settings = new();
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("files/appsettings.json", optional: false, false)
                .AddJsonFile($"files/appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, true)
                .Build();
        configuration.Bind(settings);
        return configuration;
    }
}
