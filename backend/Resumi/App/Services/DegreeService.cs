using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class DegreeService : IDegreeService
{
    public Task<Result<Degree>> CreateAsync(Degree? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Degree>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Degree>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Degree>> UpdateAsync(Degree? current, Degree? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
