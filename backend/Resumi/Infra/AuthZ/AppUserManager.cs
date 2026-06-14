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
	/// <summary>
	/// Separador usado para diferenciar o nome do usuário e um sufixo numérico gerado a partir do ID do usuário.
	/// </summary>
	private const char UserNameSeparator = '.';

	/// <summary>
	/// Define o valor mínimo para o sufixo numérico gerado a partir do ID do usuário.
	/// </summary>
	private const uint MinHashValue = 1000;

	/// <summary>
	/// Define o valor máximo para o sufixo numérico gerado a partir do ID do usuário.
	/// </summary>
	private const uint MaxHashValue = 9000;

	/// <summary>
	/// Opções de configuração para o Identity Core.
	/// </summary>
	public static Action<IdentityOptions> IdentityOptionsSetup => options =>
	{
		options.User.RequireUniqueEmail = true;
		options.Password.RequiredLength = 8;
	};

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

	/// <summary>
	/// Gera um nome de usuário único baseado no nome completo do usuário e um sufixo numérico derivado do ID do usuário.
	/// </summary>
	/// <param name="user">O usuário para gerar o novo nome de usuário</param>
	/// <remarks>
	/// <list type="bullet">É necessário que o usuário já tenha seu ID definido para garantir que o sufixo numérico seja consistente e único.</list>
	/// <list type="bullet">O nome de usuário gerado seguirá o padrão: [nome].[sufixo]. Ex.: `johndoe.1234`</list>
	/// </remarks>
	/// <exception cref="InvalidOperationException">Se o usuário ainda não tiver seu ID definido</exception>
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

	/// <summary>
	/// Normaliza um nome, removendo acentos, caracteres especiais e convertendo para minúsculas, garantindo que o nome resultante contenha apenas caracteres alfanuméricos.
	/// </summary>
	/// <param name="name">O nome a ser normalizado</param>
	/// <returns>O nome normalizado</returns>
	/// <exception cref="ArgumentException">Se o nome for nulo ou vazio</exception>
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

	/// <summary>
	/// Gera uma expressão regular gerada para corresponder a todos os caracteres alfanuméricos (isto é, que sejam somente letras ou números).
	/// </summary>
	/// <returns>Uma instância de <see cref="Regex"/>, representando a expressão regular.</returns>
	[GeneratedRegex("[^a-zA-Z0-9]")]
	private static partial Regex AlphanumericOnly();
}