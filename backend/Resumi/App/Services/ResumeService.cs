using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.App.Validators;
using Resumi.Domain.Models;
using Resumi.Domain.Validators;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Parameters;

namespace Resumi.App.Services;

public class ResumeService(
    ResumeValidator domainValidator,
    ResumeQueryValidator queryValidator,
    UserQueryValidator userQueryValidator,
    ILogger<ResumeService> logger,
    AppDbContext dbContext)
{
    public async Task<Result<Resume>> CreateAsync(Resume? newResume)
    {
        try
        {
            var validationResult = domainValidator.ValidateCreation(newResume);

            if (!validationResult.Succeeded)
            {
                return Result<Resume>.Failure(validationResult.Errors);
            }

            var createdResume = await dbContext.Resumes.AddAsync(newResume!);

            if (createdResume.State is not EntityState.Added)
            {
                return Result<Resume>.Failure(nameof(Resume), Resume.FailedToCreate);
            }

            await dbContext.SaveChangesAsync();

            return Result<Resume>.Success(newResume!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create '{Type}' entity: {Message}", nameof(Resume), ex.Message);

            return Result<Resume>.Failure(nameof(Resume), Resume.FailedToCreate);
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
                ? Result<ResumeModel>.Failure(nameof(Resume), Resume.InvalidState)
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
                ? Result<List<ResumeModel>>.Failure(nameof(Resume), Resume.NotFound)
                : Result<List<ResumeModel>>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to find '{Type}' collections for user '{User}': {Message}", nameof(Resume),
                userId, ex.Message);

            return Result<List<ResumeModel>>.Failure(nameof(Resume), Resume.FailedToQuery);
        }
    }

    public Task<Result<Resume>> UpdateAsync(Resume? current, Resume? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Resume>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }
}