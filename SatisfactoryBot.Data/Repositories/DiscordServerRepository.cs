namespace SatisfactoryBot.Data.Repositories;

using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using System.Linq;

public class DiscordServerRepository : GenericRepository<DiscordEntity>, IDiscordServerRepository
{
    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;

    public DiscordServerRepository(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork.DbContext)
    {
        this.unitOfWork = unitOfWork;
    }

    public SatisfactoryServer GetActiveSatisfactoryFromDiscordEntityId(ulong entityId) =>
        unitOfWork.GetRepository<DiscordEntity>().GetAll()
            .Where(s => s.EntityId == entityId)
            .SelectMany(d => d.SatisfactoryServers)
            .FirstOrDefault(s => s.IsDefaultServer);

    public List<SatisfactoryServer> GetSatisfactoryServersListFromDiscordEntityId(ulong entityId) =>
        unitOfWork.GetRepository<DiscordEntity>().GetAll()
            .Where(s => s.EntityId == entityId)
            .SelectMany(d => d.SatisfactoryServers).ToList();

    public DiscordEntity GetOrCreateDiscordEntity(ulong entityId)
    {
        var discordRepository = unitOfWork.GetRepository<DiscordEntity>();
        var discordServer = discordRepository.GetAll().FirstOrDefault(d => d.EntityId == entityId);
        if (discordServer == null)
        {
            discordServer = new DiscordEntity()
            {
                EntityId = entityId
            };
            discordRepository.Add(discordServer);
        }
        return discordServer;
    }

    #region Public Inheritance

    #endregion Public Inheritance
}
