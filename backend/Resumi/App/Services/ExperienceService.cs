using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class ExperienceService : IExperienceService
{
    public Task<Result<Experience>> CreateAsync(Experience? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Experience>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Experience>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Experience>> UpdateAsync(Experience? current, Experience? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}