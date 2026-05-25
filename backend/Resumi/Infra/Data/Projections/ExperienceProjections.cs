using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;

namespace Resumi.Infra.Data.Projections;

using ExperienceProjection = Expression<Func<Experience, ExperienceModel>>;

public static class ExperienceProjections
{
	public static readonly ExperienceProjection Basic = e => new ExperienceModel
	{
		Id = e.Id,
		ResumeId = e.ResumeId,
		Name = e.Name,
		Description = e.Description,
		InstitutionName = e.InstitutionName,
		Location = e.Location,
		IsRemote = e.IsRemote,
		StartDate = e.StartDate,
		EndDate = e.EndDate,
		StillEngaged = e.StillEngaged,
	};
}