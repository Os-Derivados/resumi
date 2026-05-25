using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;

namespace Resumi.Infra.Data.Projections;

using CertificateProjection = Expression<Func<Certificate, CertificateModel>>;

public static class CertificateProjections
{
	public static readonly CertificateProjection Basic = c => new CertificateModel
	{
		Id = c.Id,
		ResumeId = c.ResumeId,
		Name = c.Name,
		Description = c.Description,
		InstitutionName = c.InstitutionName,
		Location = c.Location,
		IsRemote = c.IsRemote,
		StartDate = c.StartDate,
		EndDate = c.EndDate,
		StillEngaged = c.StillEngaged,
		Type = c.Type.ToDisplayString(),
		CredentialId = c.CredentialId,
		CredentialUrl = c.CredentialUrl
	};
}