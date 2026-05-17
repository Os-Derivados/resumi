using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.App.Exceptions;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Mappers;

public class CertificateMapper(ILogger<CertificateMapper> logger) : ICertificateMapper
{
	public Certificate? NewDomainModel(AddCertificateModel dtoCreate)
	{
		try
		{
			return new Certificate
			{
				ResumeId = dtoCreate.ResumeId,
				Name = dtoCreate.Name,
				Description = dtoCreate.Description,
				InstitutionName = dtoCreate.InstitutionName,
				Location = dtoCreate.Location,
				IsRemote = dtoCreate.IsRemote,
				StartDate = dtoCreate.StartDate,
				StillEngaged = dtoCreate.StillEngaged,
				EndDate = dtoCreate.EndDate,
				CredentialId = dtoCreate.CredentialId,
				CredentialUrl = dtoCreate.CredentialUrl,
				Type = CertificateTypeExtensions.FromDisplayString(dtoCreate.Type, out var parsedType)
					? parsedType!.Value
					: throw new DomainException($"Invalid certificate type: {dtoCreate.Type}")
			};
		}
		catch (DomainException dex)
		{
			logger.LogError(
				dex,
				"Domain exception occurred while creating a new Certificate domain model: {Message}",
				dex.Message
			);

			return null;
		}
	}

	public CertificateModel? ToDto(Certificate? entity)
	{
		try
		{
			if (entity is null) return null;

			return new CertificateModel
			{
				Id = entity.Id,
				ResumeId = entity.ResumeId,
				Name = entity.Name,
				Description = entity.Description,
				InstitutionName = entity.InstitutionName,
				Location = entity.Location,
				IsRemote = entity.IsRemote,
				StartDate = entity.StartDate,
				EndDate = entity.EndDate,
				StillEngaged = entity.StillEngaged,
				CredentialId = entity.CredentialId,
				CredentialUrl = entity.CredentialUrl,
				Type = entity.Type.ToDisplayString(out var displayString)
					? displayString!
					: throw new InfrastructureException($"Certificate type not mapped: {entity.Type}"),
			};
		}
		catch (InfrastructureException iex)
		{
			logger.LogError(
				iex,
				"Infrastructure exception occurred while mapping Certificate domain model to DTO: {Message}",
				iex.Message
			);

			return null;
		}
	}

	public Certificate? UpdatedDomainModel(UpdateCertificateModel? dtoUpdate, Certificate? entity)
	{
		if (dtoUpdate is null || entity is null) return null;

		try
		{
			if (dtoUpdate.Id != entity.Id)
				throw new DomainException(
					$"The ID of the update model ({dtoUpdate.Id}) does not match the ID of the entity ({entity.Id})."
				);

			var hasParsedType = CertificateTypeExtensions.FromDisplayString(dtoUpdate.Type, out var parsedType);

			return new Certificate
			{
				#region Non-exposed properties

				ResumeId = entity.ResumeId,
				CreatedAt = entity.CreatedAt,
				UpdatedAt = entity.UpdatedAt,

				#endregion

				Name = dtoUpdate.Name,
				Description = dtoUpdate.Description,
				InstitutionName = dtoUpdate.InstitutionName,
				Location = dtoUpdate.Location,
				IsRemote = dtoUpdate.IsRemote ?? entity.IsRemote,
				StartDate = dtoUpdate.StartDate ?? entity.StartDate,
				StillEngaged = dtoUpdate.StillEngaged ?? entity.StillEngaged,
				EndDate = dtoUpdate.EndDate ?? entity.EndDate,
				CredentialId = dtoUpdate.CredentialId ?? entity.CredentialId,
				CredentialUrl = dtoUpdate.CredentialUrl ?? entity.CredentialUrl,
				Type = dtoUpdate.Type is not null
					? hasParsedType
						? parsedType!.Value
						: throw new DomainException($"Invalid certificate type: {dtoUpdate.Type}")
					: entity.Type
			};
		}
		catch (DomainException dex)
		{
			logger.LogError(
				dex,
				"Domain exception occurred while creating a new Certificate domain model: {Message}",
				dex.Message
			);

			return null;
		}
	}
}

static class CertificateTypeExtensions
{
	public static bool ToDisplayString(this CertificateType type, out string? displayString)
	{
		displayString = type switch
		{
			CertificateType.Course => "course",
			CertificateType.License => "license",
			CertificateType.Badge => "badge",
			CertificateType.Extracurricular => "extracurricular",
			CertificateType.Nomination => "nomination",
			_ => null
		};

		return displayString != null;
	}

	public static bool FromDisplayString(string? displayString, out CertificateType? type)
	{
		type = displayString?.ToLower() switch
		{
			"course" => CertificateType.Course,
			"license" => CertificateType.License,
			"badge" => CertificateType.Badge,
			"extracurricular" => CertificateType.Extracurricular,
			"nomination" => CertificateType.Nomination,
			_ => null
		};

		return type != null;
	}
}