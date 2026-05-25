using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;

namespace Resumi.Infra.Data.Projections;

using DegreeProjection = Expression<Func<Degree, DegreeModel>>;

/// <summary>
/// Objeto de parâmetros para gerar projeções em consultas somente-leitura de entidades <see cref="Degree"/>  
/// </summary>
public static class DegreeProjections
{
	public static readonly DegreeProjection Basic = d => new DegreeModel
	{
		Id = d.Id,
		ResumeId = d.ResumeId,
		Name = d.Name,
		Description = d.Description,
		InstitutionName = d.InstitutionName,
		Location = d.Location,
		IsRemote = d.IsRemote,
		StartDate = d.StartDate,
		EndDate = d.EndDate,
		StillEngaged = d.StillEngaged,
		Level = d.Level.ToDisplayString()
	};
}