using Microsoft.EntityFrameworkCore;
using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.Infra.Database.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly AppDbContext Context;
    protected readonly ILogger<Repository<TEntity>> Logger;

    protected Repository(AppDbContext context, ILogger<Repository<TEntity>> logger)
    {
        Context = context;
        Logger = logger;
    }

    public DbSet<TEntity> Data => Context.Set<TEntity>();

    public Task<TEntity?> GetByIdAsync(int id)
    {
        try
        {
            return Data.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while retrieving entity via: {Id} - {ErrorMessage}",
                id,
                ex.Message
            );

            return Task.FromResult<TEntity?>(null);
        }
    }

    public Task<IEnumerable<TEntity>?> GetAllAsync(int skip = 0, int take = 100)
    {
        try
        {
            return Task.FromResult<IEnumerable<TEntity>?>(
                Data.AsNoTracking().Skip(skip).Take(take).OfType<TEntity>()
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while retrieving all entities - {ErrorMessage}",
                ex.Message
            );

            return Task.FromResult<IEnumerable<TEntity>?>(null);
        }
    }

    public async Task<TEntity?> AddAsync(TEntity entity)
    {
        try
        {
            var addResult = await Data.AddAsync(entity);

            return addResult.Entity;
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while adding entity: {Entity} - {ErrorMessage}",
                entity,
                ex.Message
            );

            return null;
        }
    }

    public Task<TEntity?> UpdateAsync(TEntity entity)
    {
        try
        {
            var updateResult = Data.Update(entity);

            return Task.FromResult<TEntity?>(updateResult.Entity);
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while updating entity: {Entity} - {ErrorMessage}",
                entity,
                ex.Message
            );

            return Task.FromResult<TEntity?>(null);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = Data.Find(id);

            if (entity == null)
                return Task.FromResult(false);

            Data.Remove(entity);

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while deleting entity via: {Id} - {ErrorMessage}",
                id,
                ex.Message
            );

            return Task.FromResult(false);
        }
    }

    public virtual Task<bool> ExistsAsync(TEntity entity)
    {
        try
        {
            return Data.AnyAsync(e => e.Id == entity.Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Error while checking existence of entity: {Entity} - {ErrorMessage}",
                entity,
                ex.Message
            );

            return Task.FromResult(false);
        }
    }
}
