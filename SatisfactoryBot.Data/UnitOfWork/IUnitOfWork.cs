namespace SatisfactoryBot.Data.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using SatisfactoryBot.Data.Models;
using SatisfactoryBot.Data.Repositories.Interfaces;
using System;
using System.Linq;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    /// <summary>
    /// Gets the db context.
    /// </summary>
    /// <returns>The instance of type TContext.</returns>
    TContext DbContext { get; }

    /// <summary>
    /// Gets the specified repository for the TEntity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from GenericRepository interface.</returns>
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseModel;

    /// <summary>
    /// Executes the specified raw SQL command.
    /// </summary>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The number rows affected.</returns>
    int ExecuteSqlCommand(string sql, params object[] parameters);

    /// <summary>
    /// Uses raw SQL queries to fetch the specified TEntity data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>An IQueryable for TEntity that contains elements that satisfy the condition specified by raw SQL.</returns>
    IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseModel;

    /// <summary>
    /// Commit all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    int Save();
}