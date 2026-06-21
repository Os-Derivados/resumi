using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;
using Resumi.Domain.Validators;

namespace Resumi.App.Services;

public class ExperienceService(
    AppDbContext dbContext,
    ILogger<ExperienceService> logger,
    ResumeNodeValidator validator)
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

            if (creationResult.State is not EntityState.Added)
            {
                return Result<ExperienceModel>.Failure(nameof(Experience), Experience.FailedToCreate);
            }

            _ = await dbContext.SaveChangesAsync();

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

            if (updateResult.State is not EntityState.Modified)
            {
                return Result<ExperienceModel>.Failure(nameof(Experience), Experience.FailedToUpdate);
            }

            _ = await dbContext.SaveChangesAsync();

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
                return Result.Failure(nameof(Experience), Entity.NotFound);
            }

            var removalResult = dbContext.Experiences.Remove(target);

            if (removalResult.State is not EntityState.Deleted)
            {
                return Result.Failure(nameof(Experience), Experience.FailedToDelete);
            }

            _ = await dbContext.SaveChangesAsync();

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete experience: {Message}", ex.Message);

            return Result.Failure(nameof(Experience), Experience.InternalError);
        }
    }
}