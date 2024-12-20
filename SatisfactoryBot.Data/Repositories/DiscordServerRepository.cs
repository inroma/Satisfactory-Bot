﻿namespace SatisfactoryBot.Data.Repositories;

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

    public SatisfactoryServer GetActiveSatisfactoryFromDiscordEntityId(ulong entityId)
    {
        var server = unitOfWork.GetRepository<DiscordEntity>().GetAll()
            .Where(s => s.EntityId == entityId)
            .SelectMany(d => d.SatisfactoryServers)
            .FirstOrDefault(s => s.IsDefaultServer);
        return server ?? throw new KeyNotFoundException("No server registered. Use /claim to add one.");
    }

    public List<SatisfactoryServer> GetSatisfactoryServersListFromDiscordEntityId(ulong entityId)
    {
        var result = unitOfWork.GetRepository<DiscordEntity>().GetAll()
            .Where(s => s.EntityId == entityId)
            .SelectMany(d => d.SatisfactoryServers).ToList();
        if (result?.Count == 0)
        {
            throw new KeyNotFoundException("No server registered. Use /claim to add one.");
        }
        return result;
    }

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
