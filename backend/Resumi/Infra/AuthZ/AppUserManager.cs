using System.Globalization;
using System.IO.Hashing;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Resumi.Domain.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.AuthZ;

public partial class AppUserManager(
	// extended
	AppDbContext context,
	// base
	IUserStore<AppUser> store,
	IOptions<IdentityOptions> optionsAccessor,
	IPasswordHasher<AppUser> passwordHasher,
	IEnumerable<IUserValidator<AppUser>> userValidators,
	IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
	ILookupNormalizer keyNormalizer,
	IdentityErrorDescriber errors,
	IServiceProvider services,
	ILogger<UserManager<AppUser>> logger) : UserManager<AppUser>(store,
	optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
{
	private const char UserNameSeparator = '.';
	private const uint MinHashValue = 1000;
	private const uint MaxHashValue = 9000;

	public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
	{
		await using var transaction = await context.Database.BeginTransactionAsync();

		try
		{
			user.UserName = user.Email!.ToUpperInvariant();

			var result = await base.CreateAsync(user, password);

			if (!result.Succeeded)
			{
				await transaction.RollbackAsync();

				return result;
			}

			GenerateUserName(user);

			var finalUserName = user.UserName;

			var counter = 1;

			while (await Users.AsNoTracking().AnyAsync(u => u.UserName == finalUserName))
			{
				finalUserName = $"{user.UserName}{UserNameSeparator}{counter}";
				counter++;
			}

			user.UserName = finalUserName;
			user.NormalizedUserName = finalUserName.ToUpperInvariant();

			var updateResult = await base.UpdateAsync(user);

			if (!updateResult.Succeeded)

			{
				await transaction.RollbackAsync();

				return updateResult;
			}

			await transaction.CommitAsync();

			return IdentityResult.Success;
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "An error occurred while creating a new user: {Message}", ex.Message);

			await transaction.RollbackAsync();

			return IdentityResult.Failed(new IdentityError
				{ Description = "An unexpected error occurred while creating the user." });
		}
	}

	private static void GenerateUserName(AppUser user)
	{
		if (user.Id is 0) throw new InvalidOperationException("User ID must be set before generating a username.");

		var parts = user.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var last = parts.Length > 1 ? parts[1] : string.Empty;

		var cleanFirst = NormalizedName(parts[0]);
		var cleanLast = last != string.Empty ? NormalizedName(last) : string.Empty;

		var prefix = !string.IsNullOrEmpty(cleanLast)
			? $"{cleanFirst}{UserNameSeparator}{cleanLast}"
			: cleanFirst;
		var inputBytes = BitConverter.GetBytes(user.Id);
		var hashBytes = XxHash32.Hash(inputBytes);
		var hashValue = BitConverter.ToUInt32(hashBytes, 0);

		var suffix = MinHashValue + (hashValue % MaxHashValue);

		user.UserName = $"{prefix}{suffix}";
	}

	private static string NormalizedName(string name)
	{
		if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		var normalized = name.Normalize(NormalizationForm.FormD);

		StringBuilder sb = new();

		foreach (var c in normalized.Where(c =>
			         CharUnicodeInfo.GetUnicodeCategory(c) is not UnicodeCategory.NonSpacingMark))
		{
			sb.Append(c);
		}

		var clean = AlphanumericOnly().Replace(sb.ToString(), "").ToLowerInvariant();

		return clean.Length is 0
			? throw new ArgumentException("Name must contain at least one alphanumeric character.", nameof(name))
			: clean;
	}

	[GeneratedRegex("[^a-zA-Z0-9]")]
	private static partial Regex AlphanumericOnly();
}