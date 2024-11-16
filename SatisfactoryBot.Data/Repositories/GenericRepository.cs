namespace SatisfactoryBot.Data.Repositories;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SatisfactoryBot.Data.Models;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseModel
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Initializes a new instance of the GenericRepository<TEntity>.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    #region CREATE

    public virtual void Add(TEntity entity)
    {
        var entry = _dbSet.Add(entity);
    }

    public virtual void Add(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

    #endregion

    #region READ

    public virtual TEntity GetById(params object[] keyValues) => _dbSet.Find(keyValues);

    public virtual TEntity GetFirstOrDefault(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include != null)
        {
            query = include(query);
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (orderBy != null)
        {
            return orderBy(query).FirstOrDefault();
        }
        else
        {
            return query.FirstOrDefault();
        }
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public virtual IEnumerable<TEntity> GetMultiple(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (include != null)
        {
            query = include(query);
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public virtual IQueryable<TEntity> FromSql(
        string sql,
        params object[] parameters
    ) => _dbSet.FromSqlRaw(sql, parameters);

    #endregion

    #region UPDATE

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    #endregion

    #region DELETE

    public virtual void Delete(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
        }
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public virtual void Delete(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    #endregion

    #region OTHER

    public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate == null)
        {
            return _dbSet.Count();
        }
        else
        {
            return _dbSet.Count(predicate);
        }
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Any(predicate);
    }

    #endregion
}
