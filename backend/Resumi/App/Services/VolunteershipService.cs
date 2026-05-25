using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Services;

public class VolunteershipService(
	AppDbContext dbContext,
	ILogger<VolunteershipService> logger,
	IDomainValidator<ResumeNode> validator)
{
	public async Task<Result<VolunteershipModel>> CreateAsync(Volunteership? newEntity)
	{
		try
		{
			var validationResult = validator.ValidateCreation(newEntity);

			if (!validationResult.Succeeded)
			{
				return Result<VolunteershipModel>.Failure(validationResult);
			}

			var creationResult = await dbContext.VolunteerExperiences.AddAsync(newEntity!);
			var changedEntries = await dbContext.SaveChangesAsync();

			if (creationResult.State is not EntityState.Added || changedEntries is 0)
			{
				return Result<VolunteershipModel>.Failure(nameof(Volunteership), Volunteership.FailedToCreate);
			}

			var createdVolunteership = await dbContext.VolunteerExperiences
				.AsNoTracking()
				.Select(VolunteershipProjections.Basic)
				.FirstOrDefaultAsync(v => v.Id == newEntity!.Id);

			return Result<VolunteershipModel>.Success(createdVolunteership!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to create Volunteership: {Message}", ex.Message);

			return Result<VolunteershipModel>.Failure(nameof(Volunteership), Volunteership.InternalError);
		}
	}

	public async Task<Result<VolunteershipModel>> UpdateAsync(Volunteership? current, Volunteership? updated)
	{
		try
		{
			var validationResult = validator.ValidateUpdate(current, updated);

			if (!validationResult.Succeeded)
			{
				return Result<VolunteershipModel>.Failure(validationResult);
			}

			var updateResult = dbContext.VolunteerExperiences.Update(updated!);
			var changedEntries = await dbContext.SaveChangesAsync();

			if (updateResult.State is not EntityState.Modified || changedEntries is 0)
			{
				return Result<VolunteershipModel>.Failure(nameof(Volunteership), Volunteership.FailedToUpdate);
			}

			var updatedVolunteership = await dbContext.VolunteerExperiences
				.AsNoTracking()
				.Select(VolunteershipProjections.Basic)
				.FirstOrDefaultAsync(v => v.Id == updated!.Id);

			return Result<VolunteershipModel>.Success(updatedVolunteership!);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to update Volunteershsip: {Message}", ex.Message);

			return Result<VolunteershipModel>.Failure(nameof(Volunteership), Volunteership.InternalError);
		}
	}

	public async Task<Result> DeleteAsync(int id)
	{
		try
		{
			var target = await dbContext.VolunteerExperiences.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

			if (target is null)
			{
				return Result.Failure(nameof(Volunteership), Volunteership.NotFound);
			}

			var removalResult = dbContext.VolunteerExperiences.Remove(target);
			var changedEntries = await dbContext.SaveChangesAsync();

			if (removalResult.State is not EntityState.Deleted || changedEntries is 0)
			{
				return Result.Failure(nameof(Volunteership), Volunteership.FailedToDelete);
			}

			return Result.Success;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to delete Volunteership: {Message}", ex.Message);

			return Result.Failure(nameof(Volunteership), Volunteership.InternalError);
		}
	}
}