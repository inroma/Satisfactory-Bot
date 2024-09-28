namespace SatisfactoryBot.Data.Repositories;

using Microsoft.EntityFrameworkCore.Query;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public abstract class DiscordServerRepository : IDiscordServerRepository
{
    private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;

    protected DiscordServerRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
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

    public abstract void Add(DiscordServer entity);
    public abstract void Add(IEnumerable<DiscordServer> entities);
    public abstract int Count(Expression<Func<DiscordServer, bool>> predicate = null);
    public abstract void Delete(object id);
    public abstract void Delete(DiscordServer entityToDelete);
    public abstract void Delete(IEnumerable<DiscordServer> entities);
    public abstract bool Exists(Expression<Func<DiscordServer, bool>> predicate);
    public abstract IQueryable<DiscordServer> FromSql(string sql, params object[] parameters);
    public abstract IQueryable<DiscordServer> GetAll();
    public abstract DiscordServer GetById(params object[] keyValues);
    public abstract DiscordServer GetFirstOrDefault(Expression<Func<DiscordServer, bool>> predicate = null, Func<IQueryable<DiscordServer>, IOrderedQueryable<DiscordServer>> orderBy = null, Func<IQueryable<DiscordServer>, IIncludableQueryable<DiscordServer, object>> include = null, bool disableTracking = true);
    public abstract IEnumerable<DiscordServer> GetMultiple(Expression<Func<DiscordServer, bool>> predicate = null, Func<IQueryable<DiscordServer>, IOrderedQueryable<DiscordServer>> orderBy = null, Func<IQueryable<DiscordServer>, IIncludableQueryable<DiscordServer, object>> include = null, bool disableTracking = true);
    public abstract void Update(DiscordServer entity);
    public abstract void Update(IEnumerable<DiscordServer> entities);

    #endregion Public Inheritance
}
