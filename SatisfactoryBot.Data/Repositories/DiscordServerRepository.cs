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

    public SatisfactoryServer GetSatisfactoryServerFromDiscordGuildId(ulong guildId)
    {
        return unitOfWork.GetRepository<DiscordServer>().GetAll()
            .Where(s => s.GuildId == guildId)
            .SelectMany(d => d.SatisfactoryServers).FirstOrDefault();
    }

    #region Public Inheritance

    #endregion Public Inheritance
}
