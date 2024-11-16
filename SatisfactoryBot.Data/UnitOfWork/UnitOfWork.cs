namespace SatisfactoryBot.Data.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Repositories.Interfaces;
using SatisfactoryBot.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using SatisfactoryBot.Data.Models;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    private bool disposed = false;
    private Dictionary<Type, object> _repositories;

    /// <summary>
    /// Initializes a new instance of the UnitOfWork<TContext>.
    /// </summary>
    /// <param name="context">The context.</param>
    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public TContext DbContext => _context;

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel
    {
        _repositories ??= [];
        var type = typeof(TEntity);
        if (!_repositories.TryGetValue(type, out object repo))
        {
            repo = new GenericRepository<TEntity>(_context);
            _repositories[type] = repo;
        }
        return (IGenericRepository<TEntity>)repo;
    }

    public int ExecuteSqlCommand(
        string sql,
        params object[] parameters
    ) => _context.Database.ExecuteSqlRaw(sql, parameters);

    public IQueryable<TEntity> FromSql<TEntity>(
        string sql,
        params object[] parameters
    ) where TEntity : BaseModel => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

    public int Save()
    {
        return _context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}