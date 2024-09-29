namespace SatisfactoryBot.Data.Repositories;

using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using System.Linq;

public class DiscordServerRepository : GenericRepository<DiscordServer>, IDiscordServerRepository
{
    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;

    public DiscordServerRepository(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork.DbContext)
    {
        this.unitOfWork = unitOfWork;
    }

    public SatisfactoryServer GetActiveSatisfactoryFromDiscordGuildId(ulong guildId) =>
        unitOfWork.GetRepository<DiscordServer>().GetAll()
            .Where(s => s.GuildId == guildId)
            .SelectMany(d => d.SatisfactoryServers)
            .FirstOrDefault(s => s.IsDefaultServer);

    public List<SatisfactoryServer> GetSatisfactoryServersListFromDiscordGuildId(ulong guildId) =>
        unitOfWork.GetRepository<DiscordServer>().GetAll()
            .Where(s => s.GuildId == guildId)
            .SelectMany(d => d.SatisfactoryServers).ToList();

    public DiscordServer GetOrCreateDiscordServer(ulong guildId)
    {
        var discordRepository = unitOfWork.GetRepository<DiscordServer>();
        var discordServer = discordRepository.GetAll().FirstOrDefault(d => d.GuildId == guildId);
        if (discordServer == null)
        {
            discordServer = new DiscordServer()
            {
                GuildId = guildId
            };
            discordRepository.Add(discordServer);
        }
        return discordServer;
    }

    #region Public Inheritance

    #endregion Public Inheritance
}
