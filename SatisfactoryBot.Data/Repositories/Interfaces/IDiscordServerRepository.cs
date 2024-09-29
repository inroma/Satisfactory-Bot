namespace SatisfactoryBot.Data.Repositories.Interfaces;

using SatisfactoryBot.Data.Models;

public interface IDiscordServerRepository: IGenericRepository<DiscordServer>
{
    SatisfactoryServer GetActiveSatisfactoryFromDiscordGuildId(ulong guildId);

    List<SatisfactoryServer> GetSatisfactoryServersListFromDiscordGuildId(ulong guildId);

    DiscordServer GetOrCreateDiscordServer(ulong guildId);
}
