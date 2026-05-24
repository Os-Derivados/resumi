using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Projections;

namespace Resumi.App.Services;

public class DegreeService(
	IDomainValidator<Degree> validator,
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
				.Select(DegreeQueries.Basic)
				.FirstOrDefaultAsync(d => d.Id == newEntity!.Id);

			return Result<DegreeModel>.Success(createdModel!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to create degree: {Message}", ex.Message);

			return Result<DegreeModel>.Failure(nameof(Degree), Degree.InternalError);
		}
	}

	public async Task<Result<DegreeModel>> FindAsync(int id)
	{
		try
		{
			return await dbContext.AcademicDegrees.AsNoTracking()
				.Select(DegreeQueries.Basic)
				.FirstOrDefaultAsync(d => d.Id == id) is { } model
				? Result<DegreeModel>.Success(model)
				: Result<DegreeModel>.Failure(nameof(Degree), Degree.NotFound);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to find degree with ID {Id}: {Message}", id, ex.Message);

			return Result<DegreeModel>.Failure(nameof(Degree), Degree.InternalError);
		}
	}

	public async Task<Result<List<DegreeModel>>> FindByResumeAsync(int resumeId, int skip = 0, int take = 20)
	{
		try
		{
			if (skip < 0 || take <= 0 || take > 100)
			{
				return Result<List<DegreeModel>>.Failure(
					nameof(Degree),
					"Parâmetros de paginação inválidos. Skip deve ser >= 0 e Take deve estar entre 1 e 100.");
			}

			var degrees = await dbContext.AcademicDegrees.AsNoTracking()
				.Select(DegreeQueries.Basic)
				.Where(d => d.ResumeId == resumeId)
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			return Result<List<DegreeModel>>.Success(degrees);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to find degrees for resume ID {ResumeId}: {Message}", resumeId, ex.Message);

			return Result<List<DegreeModel>>.Failure(nameof(Degree), Degree.InternalError);
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
				.Select(DegreeQueries.Basic)
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
