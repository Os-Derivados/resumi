using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.App.Services;

public class ExperienceService(
    AppDbContext dbContext,
    ILogger<ExperienceService> logger,
    IDomainValidator<Experience> validator)
{
    public async Task<Result<ExperienceModel>> CreateAsync(Experience? newEntity)
    {
        try
        {
            var validationResult = validator.ValidateCreation(newEntity);

            if (!validationResult.Succeeded)
            {
                return Result<ExperienceModel>.Failure(validationResult);
            }

            var creationResult = await dbContext.Experiences.AddAsync(newEntity!);
            var createdEntries = await dbContext.SaveChangesAsync();

            if (creationResult.State is not EntityState.Added || createdEntries == 0)
            {
                return Result<ExperienceModel>.Failure(nameof(Experience), Experience.FailedToCreate);
            }

            var createdExperience = await dbContext.Experiences.AsNoTracking()
                .Select(ExperienceProjections.Basic).FirstOrDefaultAsync(e => e.Id == newEntity!.Id);

            return Result<ExperienceModel>.Success(createdExperience!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create experience: {Message}", ex.Message);

            return Result<ExperienceModel>.Failure(nameof(Experience), Experience.InternalError);
        }
    }

    public async Task<Result<ExperienceModel>> UpdateAsync(Experience? current, Experience? updated)
    {
        try
        {
            var validationResult = validator.ValidateUpdate(current, updated);

            if (!validationResult.Succeeded)
            {
                return Result<ExperienceModel>.Failure(validationResult);
            }

            var updateResult = dbContext.Experiences.Update(updated!);
            var updatedEntries = await dbContext.SaveChangesAsync();

            if (updateResult.State is not EntityState.Modified || updatedEntries is 0)
            {
                return Result<ExperienceModel>.Failure(nameof(Experience), Experience.FailedToUpdate);
            }

            var updatedExperience = await dbContext.Experiences
                .AsNoTracking()
                .Select(ExperienceProjections.Basic)
                .FirstOrDefaultAsync(e => e.Id == updated!.Id);

            return Result<ExperienceModel>.Success(updatedExperience!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update experience: {Message}", ex.Message);

            return Result<ExperienceModel>.Failure(nameof(Experience), Experience.InternalError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var target = await dbContext.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            if (target is null)
            {
                return Result.Failure(nameof(Experience), Experience.NotFound);
            }

            var removalResult = dbContext.Experiences.Remove(target);
            var removedEntries = await dbContext.SaveChangesAsync();

            if (removalResult.State is not EntityState.Deleted || removedEntries is 0)
            {
                return Result.Failure(nameof(Experience), Experience.FailedToDelete);
            }

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete experience: {Message}", ex.Message);

            return Result.Failure(nameof(Experience), Experience.InternalError);
        }
    }
}