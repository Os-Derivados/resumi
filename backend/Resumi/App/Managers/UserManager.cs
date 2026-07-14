using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Managers;

public class UserManager(
	AppUserManager identityManager,
	AppDbContext dbContext,
	ILogger<UserManager> logger)
{
	public async Task<Result<UserModel>> FindAsync(int id, CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

		try
		{
			return await dbContext.Users
				.Where(u => u.Id == id)
				.Select(UserProjections.Basic)
				.FirstOrDefaultAsync(cancellation) is { } user
				? Result<UserModel>.Success(user)
				: Result<UserModel>.Failure(nameof(AppUser), AppUser.NotFound);
		}
		catch (OperationCanceledException)
		{
			logger.LogCritical("'{Manager}.{MethodName}' operation canceled.", nameof(UserManager),
				nameof(FindAsync));

			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error finding user with ID {UserId}: {Message}", id, ex.Message);

			return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
		}
	}

	public async Task<Result<List<UserModel>>> FindAllAsync(CancellationToken cancellation, int skip = 0, int take = 20)
	{
		cancellation.ThrowIfCancellationRequested();

		try
		{
			var users = await dbContext.Users
				.OrderBy(u => u.Id)
				.Skip(skip)
				.Take(take)
				.Select(UserProjections.Basic)
				.ToListAsync(cancellation);

			return Result<List<UserModel>>.Success(users);
		}
		catch (OperationCanceledException)
		{
			logger.LogCritical("'{Manager}.{MethodName}' operation canceled.", nameof(UserManager),
				nameof(FindAllAsync));

			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error finding all users: {Message}", ex.Message);

			return Result<List<UserModel>>.Failure(nameof(AppUser), AppUser.InternalError);
		}
	}

	public async Task<Result<UserModel>> UpdateAsync(AppUser? current, AppUser? updated,
		CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

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

			var updateResult = await identityManager.UpdateAsync(updated);

			if (!updateResult.Succeeded)
			{
				ResultErrors errors = [];

				foreach (var error in updateResult.Errors)
				{
					errors.AddError(error.Code, error.Description);
				}

				return Result<UserModel>.Failure(errors);
			}

			var updatedUser = await dbContext.Users.Where(u => u.Id == updated.Id)
				.Select(UserProjections.Basic)
				.FirstOrDefaultAsync(cancellation);

			return Result<UserModel>.Success(updatedUser!);
		}
		catch (OperationCanceledException)
		{
			logger.LogCritical("'{Manager}.{MethodName}' canceled.", nameof(UserManager), nameof(UpdateAsync));

			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error updating user: {Message}", ex.Message);

			return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
		}
	}

	public async Task<Result> DeleteAsync(int id, CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

		try
		{
			var target = await identityManager.FindByIdAsync(id.ToString());

			if (target is null)
			{
				return Result.Failure(nameof(AppUser), AppUser.NotFound);
			}

			var deleteResult = await identityManager.DeleteAsync(target);

			if (deleteResult.Succeeded) return Result.Success;

			ResultErrors errors = [];

			foreach (var error in deleteResult.Errors)
			{
				errors.AddError(error.Code, error.Description);
			}

			return Result.Failure(errors);
		}
		catch (OperationCanceledException)
		{
			logger.LogCritical("'{Manager}.{MethodName}' canceled.", nameof(UserManager), nameof(DeleteAsync));

			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error deleting user with ID {UserId}: {Message}", id, ex.Message);

			return Result.Failure(nameof(AppUser), AppUser.InternalError);
		}
	}

	public async Task<Result<UserModel>> CreateAsync(AppUser? newEntity, string password,
		CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

		try
		{
			var existingUser = await identityManager.FindByEmailAsync(newEntity?.Email ?? string.Empty);

			if (newEntity is not null && existingUser is not null)
			{
				return Result<UserModel>.Failure(
					nameof(AppUser),
					"User with the same email already exists."
				);
			}

			var creationResult = await identityManager.CreateAsync(newEntity!, password);

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
				.FirstOrDefaultAsync(u => u.Id == newEntity!.Id, cancellation);

			return Result<UserModel>.Success(createdEntity!);
		}
		catch (OperationCanceledException)
		{
			logger.LogCritical("'{Manager}.{MethodName}' was canceled", nameof(UserManager), nameof(CreateAsync));

			throw;
		}
		catch (ArgumentNullException ex)
		{
			logger.LogError(ex, "Null source found while creating user: {Source}", ex.ParamName);

			return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
		}
		catch (DbUpdateConcurrencyException)
		{
			logger.LogError("Concurrency error occurred while creating user.");

			return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error creating user: {Message}", ex.Message);

			return Result<UserModel>.Failure(nameof(AppUser), AppUser.InternalError);
		}
	}
}
