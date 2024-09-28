namespace SatisfactoryBot.Extensions;

using Microsoft.Extensions.DependencyInjection;
using SatisfactoryBot.Data.Repositories;
using SatisfactoryBot.Data.Repositories.Interfaces;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IDiscordServerRepository, DiscordServerRepository>();
}
