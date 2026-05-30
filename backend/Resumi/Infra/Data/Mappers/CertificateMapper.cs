using Resumi.Api.Data.Models;
using Resumi.App.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class CertificateMapper
	: IEntityMapper<Certificate, CertificateModel, AddCertificateModel, UpdateCertificateModel>
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
				EndDate = dtoCreate.EndDate,
				StillEngaged = dtoCreate.StillEngaged,
				CredentialId = dtoCreate.CredentialId,
				CredentialUrl = dtoCreate.CredentialUrl,
				Type = CertificateTypeExtensions.FromDisplayString(dtoCreate.Type)
			};
		}
		catch (DomainException)
		{
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
				Type = entity.Type.ToDisplayString()
			};
		}
		catch
		{
			return null;
		}
	}

	public Certificate? UpdatedDomainModel(UpdateCertificateModel? dtoUpdate, Certificate? entity)
	{
		try
		{
			if (dtoUpdate is null || entity is null) return null;

			var shallowCopy = entity.ShallowCopy();

			shallowCopy.Name = dtoUpdate.Name ?? shallowCopy.Name;
			shallowCopy.Description = dtoUpdate.Description ?? shallowCopy.Description;
			shallowCopy.InstitutionName = dtoUpdate.InstitutionName ?? shallowCopy.InstitutionName;
			shallowCopy.Location = dtoUpdate.Location ?? shallowCopy.Location;
			shallowCopy.IsRemote = dtoUpdate.IsRemote ?? shallowCopy.IsRemote;
			shallowCopy.StartDate = dtoUpdate.StartDate ?? shallowCopy.StartDate;
			shallowCopy.EndDate = dtoUpdate.EndDate ?? shallowCopy.EndDate;
			shallowCopy.StillEngaged = dtoUpdate.StillEngaged ?? shallowCopy.StillEngaged;
			shallowCopy.CredentialId = dtoUpdate.CredentialId ?? shallowCopy.CredentialId;
			shallowCopy.CredentialUrl = dtoUpdate.CredentialUrl ?? shallowCopy.CredentialUrl;
			shallowCopy.Type = CertificateTypeExtensions.TryGetValue(dtoUpdate.Type, out var type)
				? type!.Value
				: shallowCopy.Type;

			return shallowCopy;
		}
		catch (DomainException)
		{
			return null;
		}
	}
}