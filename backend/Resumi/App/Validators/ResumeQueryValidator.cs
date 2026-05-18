using Microsoft.EntityFrameworkCore;
using Resumi.App.Validators.Interfaces;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.App.Validators;

public class ResumeQueryValidator(AppDbContext dbContext, ILogger<ResumeQueryValidator> logger) : IQueryValidator
{
	public async Task<Result> ValidateSearch(int id)
	{
		try
		{
			var exists = await dbContext.Resumes.AsNoTracking().AnyAsync(r => r.Id == id);

			return exists ? Result.Success : Result.Failure(nameof(Resume), Resume.NotFound);
		}
		catch (Exception ex)
		{
			logger.LogCritical(ex, "Failed to validate '{Type}' search: {Message}", nameof(Resume), ex.Message);

			return Result.Failure(nameof(Resume), Resume.FailedToQuery);
		}
	}
}