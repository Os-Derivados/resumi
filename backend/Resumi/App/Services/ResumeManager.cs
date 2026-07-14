using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.App.Validators;
using Resumi.Domain.Models;
using Resumi.Domain.Validators;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Services;

public class ResumeManager(
	ResumeValidator domainValidator,
	ResumeQueryValidator queryValidator,
	UserQueryValidator userQueryValidator,
	ILogger<ResumeManager> logger,
	AppDbContext dbContext)
{
	public async Task<Result<ResumeModel>> CreateAsync(Resume? newResume)
	{
		try
		{
			var validationResult = domainValidator.ValidateCreation(newResume);

			if (!validationResult.Succeeded)
			{
				return Result<ResumeModel>.Failure(validationResult.Errors!);
			}

			var createdResume = await dbContext.Resumes.AddAsync(newResume!);

			if (createdResume.State is not EntityState.Added)
			{
				return Result<ResumeModel>.Failure(nameof(Resume), Resume.FailedToCreate);
			}

			_ = await dbContext.SaveChangesAsync();

			var createdModel = await dbContext.Resumes.AsNoTracking().Select(ResumeProjections.Basic)
				.FirstOrDefaultAsync(r => r.Id == newResume!.Id);

			return Result<ResumeModel>.Success(createdModel!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to create '{Type}' entity: {Message}", nameof(Resume), ex.Message);

			return Result<ResumeModel>.Failure(nameof(Resume), Entity.InvalidState);
		}
	}

	public async Task<Result<ResumeModel>> FindAsync(int id, ResumeProjectionMode projectionMode)
	{
		try
		{
			var validationResult = await queryValidator.ValidateSearch(id);

			if (!validationResult.Succeeded) return Result<ResumeModel>.Failure(validationResult);

			var result = await dbContext.Resumes.AsNoTracking().Select(projectionMode.ToProjection())
				.FirstOrDefaultAsync(r => r.Id == id);

			return result is null
				? Result<ResumeModel>.Failure(nameof(Resume), Entity.InvalidState)
				: Result<ResumeModel>.Success(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to search '{Type}' entity: {Message}", nameof(Resume), ex.Message);

			return Result<ResumeModel>.Failure(nameof(Resume), Resume.FailedToQuery);
		}
	}

	public async Task<Result<List<ResumeModel>>> FindByUserAsync(int userId, ResumeProjectionMode projectionMode,
		int skip = 0, int take = 20)
	{
		try
		{
			var validationResult = await userQueryValidator.ValidateSearch(userId);

			if (!validationResult.Succeeded) return Result<List<ResumeModel>>.Failure(validationResult);

			var result = await dbContext.Resumes
				.AsNoTracking()
				.Select(projectionMode.ToProjection())
				.Where(r => r.UserId == userId)
				.OrderBy(r => r.UserId)
				.ThenBy(r => r.Id)
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			return result.Count is 0
				? Result<List<ResumeModel>>.Failure(nameof(Resume), Entity.NotFound)
				: Result<List<ResumeModel>>.Success(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to find '{Type}' collections for user '{User}': {Message}", nameof(Resume),
				userId, ex.Message);

			return Result<List<ResumeModel>>.Failure(nameof(Resume), Resume.FailedToQuery);
		}
	}

	public async Task<Result<ResumeModel>> UpdateAsync(Resume? current, Resume? updated)
	{
		try
		{
			var validationResult = domainValidator.ValidateUpdate(current, updated);

			if (!validationResult.Succeeded) return Result<ResumeModel>.Failure(validationResult);

			var updateResult = dbContext.Resumes.Update(updated!);

			if (updateResult.State is not EntityState.Modified)
			{
				return Result<ResumeModel>.Failure(nameof(Resume), Resume.FailedToUpdate);
			}

			_ = await dbContext.SaveChangesAsync();

			var updatedModel = await dbContext.Resumes
				.AsNoTracking()
				.Select(ResumeProjections.Basic)
				.FirstOrDefaultAsync(r => r.Id == updated!.Id);

			return Result<ResumeModel>.Success(updatedModel!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to update '{Type}': {Message}", nameof(Resume), ex.Message);

			return Result<ResumeModel>.Failure(nameof(Resume), Entity.InvalidState);
		}
	}

	public async Task<Result> DeleteAsync(int id)
	{
		try
		{
			var target = await dbContext.Resumes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
			var validationResult = domainValidator.ValidateDeletion(target);

			if (!validationResult.Succeeded) return Result.Failure(validationResult);

			var deleteResult = dbContext.Resumes.Remove(target!);

			if (deleteResult.State is not EntityState.Deleted)
			{
				return Result.Failure(nameof(Resume), Resume.FailedToDelete);
			}

			_ = await dbContext.SaveChangesAsync();

			return Result.Success;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to delete '{Type}': {Message}", nameof(Resume), ex.Message);

			return Result.Failure(nameof(Resume), Entity.InvalidState);
		}
	}
}