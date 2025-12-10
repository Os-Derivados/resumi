using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

public interface IDomainService<TEntity> where TEntity : ITrackable
{
    Task<Result<TEntity>> CreateAsync(TEntity? newEntity);

    Task<Result<TEntity>> FindAsync(int id);

    Task<Result<IEnumerable<TEntity>>> FindAllAsync(int skip = 0, int take = 20);

    Task<Result<TEntity>> UpdateAsync(TEntity? current, TEntity? updated);

    Task<Result<bool>> DeleteAsync(int id);
}