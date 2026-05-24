using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Projections;

using ResumeProjection = Expression<Func<Resume, ResumeModel>>;

/// <summary>
/// Objeto de parâmetros para gerar projeções em consultas somente-leitura de entidades <see cref="Resume"/>  
/// </summary>
public static class ResumeQueries
{
	/// <summary>
	/// Seleciona o tipo de projeção a partir do tipo de consulta fornecido. 
	/// </summary>
	/// <param name="mode">O tipo de consulta fornecido.</param>
	/// <returns>Uma projeção para entidades <see cref="Resume"/>.</returns>
	/// <exception cref="InfrastructureException">Se o tipo fornecido não foi devidamente mapeado para projeção.</exception>
	public static ResumeProjection ToProjection(this ResumeProjectionMode mode)
	{
		return mode switch
		{
			ResumeProjectionMode.Basic => Basic,
			ResumeProjectionMode.Full => Full,
			_ => throw new InfrastructureException($"{nameof(ResumeProjectionMode)} not mapped for projection.")
		};
	}

	/// <summary>
	/// Entidade sem relações.
	/// </summary>
	public static readonly ResumeProjection Basic = r => new ResumeModel
	{
		Id = r.Id,
		UserId = r.UserId,
		Title = r.Title,
		OwnerName = r.OwnerName,
		Location = r.Location,
		Email = r.Email,
		PhoneNumber = r.PhoneNumber
	};

	/// <summary>
	/// Entidade com todas as relações incluídas.
	/// </summary>
	public static readonly ResumeProjection Full = r => new ResumeModel
	{
		Id = r.Id,
		UserId = r.UserId,
		Title = r.Title,
		OwnerName = r.OwnerName,
		Location = r.Location,
		Email = r.Email,
		PhoneNumber = r.PhoneNumber,
		Degrees = r.AcademicDegrees!.Select(d => new DegreeModel
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
			Highlights = d.Highlights,
			Level = d.Level.ToString()
		}).ToArray(),
		Experiences = r.Experiences!.Select(e => new ExperienceModel
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
			Highlights = e.Highlights
		}).ToArray(),
		Volunteerships = r.VolunteerExperiences!.Select(v => new VolunteershipModel
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
		}).ToArray()
	};
}

/// <summary>
/// O tipo de projeção a se fazer para entidades <see cref="Resume"/>.
/// </summary>
public enum ResumeProjectionMode
{
	/// <summary>
	/// Contém os campos básicos da entidade, sem relacionamentos incluídos.
	/// </summary>
	Basic = 0,

	/// <summary>
	/// Contém os campos básicos da entidade, juntamente de todos os relacionamentos.
	/// </summary>
	Full = 1
}