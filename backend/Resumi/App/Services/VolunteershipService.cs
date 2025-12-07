using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class VolunteershipService : IVolunteershipService
{
    public Task<Result<Volunteership>> CreateAsync(Volunteership? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Volunteership>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Volunteership>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Volunteership>> UpdateAsync(Volunteership? current, Volunteership? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}