using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class UserService : IUserService
{
    public Task<Result<AppUser>> CreateAsync(AppUser? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppUser>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<AppUser>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppUser>> UpdateAsync(AppUser? current, AppUser? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
