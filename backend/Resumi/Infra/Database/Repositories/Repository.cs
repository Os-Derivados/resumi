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
                "Error while retrieveing entity via: {Id} - {ErrorMesssage}",
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
                "Error while retrieving all entities - {ErrorMesssage}",
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
                "Error while adding entity: {Entity} - {ErrorMesssage}",
                entity,
                ex.Message
            );

            return null;
        }
    }

    public Task<TEntity?> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
