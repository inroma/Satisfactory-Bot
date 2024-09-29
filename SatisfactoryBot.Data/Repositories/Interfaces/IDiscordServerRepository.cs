namespace SatisfactoryBot.Data.Repositories.Interfaces;

using SatisfactoryBot.Data.Models;

public interface IDiscordServerRepository: IGenericRepository<DiscordServer>
{
    SatisfactoryServer GetSatisfactoryServerFromDiscordGuildId(ulong guildId);

    DiscordServer GetOrCreateDiscordServer(ulong guildId);
}
