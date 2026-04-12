using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.App.Exceptions;
using Resumi.Infra.Data.Interfaces;

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
		throw new NotImplementedException();
	}

	public Certificate? UpdatedDomainModel(UpdateCertificateModel? dtoUpdate, Certificate? entity)
	{
		throw new NotImplementedException();
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

	public static bool FromDisplayString(string displayString, out CertificateType? type)
	{
		type = displayString.ToLower() switch
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