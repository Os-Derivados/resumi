using Microsoft.EntityFrameworkCore;
using Resumi.App.Validators.Interfaces;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.App.Validators;

public class UserQueryValidator(ILogger<UserQueryValidator> logger, AppDbContext dbContext) : IQueryValidator
{
	public const string UserNotFound = "Usuário não encontrado";

	public async Task<Result> ValidateSearch(int id)
	{
		try
		{
			var exists = await dbContext.Users
				.AsNoTracking()
				.AnyAsync(u => u.Id == id);

			return exists ? Result.Success : Result.Failure(nameof(AppUser), UserNotFound);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Bla");

			return Result.Failure(nameof(AppUser), "Failed to validate user search");
		}
	}
}