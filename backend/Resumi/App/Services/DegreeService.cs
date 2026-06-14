using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Domain.Validators;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Services;

public class DegreeService(
	DegreeValidator validator,
	ILogger<DegreeService> logger,
	AppDbContext dbContext)
{
	public async Task<Result<DegreeModel>> CreateAsync(Degree? newEntity)
	{
		try
		{
			var validationResult = validator.ValidateCreation(newEntity);

			if (!validationResult.Succeeded)
			{
				return Result<DegreeModel>.Failure(validationResult.Errors!);
			}

			var creationResult = await dbContext.AcademicDegrees.AddAsync(newEntity!);
			var createdEntries = await dbContext.SaveChangesAsync();

			if (creationResult.State is not EntityState.Added || createdEntries < 1)
			{
				return Result<DegreeModel>.Failure(nameof(Degree), Degree.FailedToCreate);
			}

			var createdModel = await dbContext.AcademicDegrees.AsNoTracking()
				.Select(DegreeProjections.Basic)
				.FirstOrDefaultAsync(d => d.Id == newEntity!.Id);

			return Result<DegreeModel>.Success(createdModel!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to create degree: {Message}", ex.Message);

			return Result<DegreeModel>.Failure(nameof(Degree), Degree.InternalError);
		}
	}

	public async Task<Result<DegreeModel>> UpdateAsync(Degree? current, Degree? updated)
	{
		try
		{
			var validationResult = validator.ValidateUpdate(current, updated);

			if (!validationResult.Succeeded)
			{
				return Result<DegreeModel>.Failure(validationResult.Errors!);
			}

			var updateResult = dbContext.AcademicDegrees.Update(updated!);
			var updatedEntities = await dbContext.SaveChangesAsync();

			if (updateResult.State is not EntityState.Modified || updatedEntities < 1)
			{
				return Result<DegreeModel>.Failure(
					nameof(Degree),
					Degree.FailedToUpdate);
			}

			var updatedModel = await dbContext.AcademicDegrees.AsNoTracking()
				.Select(DegreeProjections.Basic)
				.FirstOrDefaultAsync(d => d.Id == updated!.Id);

			return Result<DegreeModel>.Success(updatedModel!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to update degree with ID {Id}: {Message}", current?.Id, ex.Message);

			return Result<DegreeModel>.Failure(nameof(Degree), Degree.InternalError);
		}
	}

	public async Task<Result> DeleteAsync(int id)
	{
		try
		{
			if (id <= 0)
			{
				return Result.Failure(nameof(Degree), Entity.InvalidPrimaryKey);
			}

			var existingEntity = await dbContext.AcademicDegrees.FindAsync(id);

			if (existingEntity is null)
			{
				return Result.Failure(nameof(Degree), Degree.NotFound);
			}

			var removalResult = dbContext.AcademicDegrees.Remove(existingEntity);
			var deletedEntries = await dbContext.SaveChangesAsync();

			if (removalResult.State is not EntityState.Deleted || deletedEntries < 1)
			{
				return Result.Failure(
					nameof(Degree),
					Degree.FailedToDelete);
			}

			return Result.Success;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to delete degree with ID {Id}: {Message}", id, ex.Message);

			return Result.Failure(nameof(Degree), Degree.InternalError);
		}
	}
}
