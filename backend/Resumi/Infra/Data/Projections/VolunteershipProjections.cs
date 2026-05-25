using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;

namespace Resumi.Infra.Data.Projections;

using VolunteershipProjection = Expression<Func<Volunteership, VolunteershipModel>>;

public static class VolunteershipProjections
{
	public static readonly VolunteershipProjection Basic = v => new VolunteershipModel
	{
		Id = v.Id,
		ResumeId = v.ResumeId,
		Name = v.Name,
		Description = v.Description,
		InstitutionName = v.InstitutionName,
		Location = v.Location,
		IsRemote = v.IsRemote,
		StartDate = v.StartDate,
		EndDate = v.EndDate,
		StillEngaged = v.StillEngaged
	};
}