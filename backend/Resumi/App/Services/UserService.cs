using Resumi.App.Data.Models;
using Resumi.App.Exceptions;
using Resumi.App.Modules;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class UserService : IUserService
{
    private readonly UsersModule _module;

    public UserService(UsersModule module)
    {
        _module = module;
    }

    public Task<Result<AppUser>> CreateAsync(AppUser? newEntity)
    {
        throw new DomainException("Use CreateAsync with password parameter for creating users.");
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

    public async Task<Result<AppUser>> CreateAsync(AppUser? newEntity, string password)
    {
        var existingUser = await _module.UserManager.FindByEmailAsync(newEntity?.Email ?? string.Empty);

        if (newEntity is not null && existingUser is not null)
        {
            return Result<AppUser>.Failure(
                nameof(AppUser),
                "User with the same email already exists."
            );
        }

        var creationResult = await _module.UserManager.CreateAsync(newEntity!, password);

        if (!creationResult.Succeeded)
        {
            return Result<AppUser>.Failure(
                nameof(AppUser),
                string.Join("; ", creationResult.Errors.Select(e => e.Description))
            );
        }

        return Result<AppUser>.Success(newEntity!);
    }
}
