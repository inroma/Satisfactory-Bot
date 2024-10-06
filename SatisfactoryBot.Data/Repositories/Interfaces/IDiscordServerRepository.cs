namespace SatisfactoryBot.Data.Repositories.Interfaces;

using SatisfactoryBot.Data.Models;

public interface IDiscordServerRepository: IGenericRepository<DiscordEntity>
{
    SatisfactoryServer GetActiveSatisfactoryFromDiscordEntityId(ulong entityId);

    List<SatisfactoryServer> GetSatisfactoryServersListFromDiscordEntityId(ulong entityId);

    DiscordEntity GetOrCreateDiscordEntity(ulong entityId);
}
