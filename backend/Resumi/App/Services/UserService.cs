using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Services;

public class UserService(
    UserManager<AppUser> userManager,
    AppDbContext dbContext,
    ILogger<UserService> logger)
{
    public async Task<Result<UserModel>> FindAsync(int id)
    {
        try
        {
            return await dbContext.Users
                .Where(u => u.Id == id)
                .Select(UserProjections.Basic)
                .FirstOrDefaultAsync() is { } user
                ? Result<UserModel>.Success(user)
                : Result<UserModel>.Failure(nameof(AppUser), AppUser.NotFound);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error finding user with ID {UserId}: {Message}", id, ex.Message);

            return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
        }
    }

    public async Task<Result<List<UserModel>>> FindAllAsync(int skip = 0, int take = 20)
    {
        try
        {
            var users = await dbContext.Users
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(take)
                .Select(UserProjections.Basic)
                .ToListAsync();

            return Result<List<UserModel>>.Success(users);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error finding all users: {Message}", ex.Message);

            return Result<List<UserModel>>.Failure(nameof(AppUser), AppUser.InternalError);
        }
    }

    public async Task<Result<UserModel>> UpdateAsync(AppUser? current, AppUser? updated)
    {
        try
        {
            if (current is null)
            {
                return Result<UserModel>.Failure(nameof(AppUser), AppUser.NotFound);
            }

            if (updated is null)
            {
                return Result<UserModel>.Failure(nameof(AppUser), AppUser.InvalidState);
            }

            if (current.Id != updated.Id)
            {
                return Result<UserModel>.Failure(nameof(AppUser), Entity.UpdatePrimaryKeyMismatch);
            }

            var updateResult = await userManager.UpdateAsync(updated);

            if (!updateResult.Succeeded)
            {
                ResultDictionary errors = [];

                foreach (var error in updateResult.Errors)
                {
                    errors.AddError(error.Code, error.Description);
                }

                return Result<UserModel>.Failure(errors);
            }

            var updatedUser = await dbContext.Users.Where(u => u.Id == updated.Id)
                .Select(UserProjections.Basic)
                .FirstOrDefaultAsync();

            return Result<UserModel>.Success(updatedUser!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user: {Message}", ex.Message);

            return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var target = await userManager.FindByIdAsync(id.ToString());

            if (target is null)
            {
                return Result.Failure(nameof(AppUser), AppUser.NotFound);
            }

            var deleteResult = await userManager.DeleteAsync(target);

            if (!deleteResult.Succeeded)
            {
                ResultDictionary errors = [];

                foreach (var error in deleteResult.Errors)
                {
                    errors.AddError(error.Code, error.Description);
                }

                return Result.Failure(errors);
            }

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting user with ID {UserId}: {Message}", id, ex.Message);

            return Result.Failure(nameof(AppUser), AppUser.InternalError);
        }
    }

    public async Task<Result<UserModel>> CreateAsync(AppUser? newEntity, string password)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(newEntity?.Email ?? string.Empty);

            if (newEntity is not null && existingUser is not null)
            {
                return Result<UserModel>.Failure(
                    nameof(AppUser),
                    "User with the same email already exists."
                );
            }

            var creationResult = await userManager.CreateAsync(newEntity!, password);

            if (!creationResult.Succeeded)
            {
                return Result<UserModel>.Failure(
                    nameof(AppUser),
                    string.Join("; ", creationResult.Errors.Select(e => e.Description))
                );
            }

            var createdEntity = await dbContext.Users
                .AsNoTracking()
                .Select(UserProjections.Basic)
                .FirstOrDefaultAsync(u => u.Id == newEntity!.Id);

            return Result<UserModel>.Success(createdEntity!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user: {Message}", ex.Message);

            return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
        }
    }
}
