using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Projections;

using ResumeProjection = Expression<Func<Resume, ResumeModel>>;

/// <summary>
/// Objeto de parâmetros para gerar projeções em consultas somente-leitura de entidades <see cref="Resume"/>  
/// </summary>
public static class ResumeProjections
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
			ResumeProjectionMode.Experiences => Experiences,
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
		Degrees = r.AcademicDegrees!.Select(DegreeProjections.Basic.Compile()).ToArray(),
		Experiences = r.Experiences!.Select(ExperienceProjections.Basic.Compile()).ToArray(),
		Volunteerships = r.VolunteerExperiences!.Select(VolunteershipProjections.Basic.Compile()).ToArray()
	};

	public static readonly ResumeProjection Experiences = r => new ResumeModel
	{
		Id = r.Id,
		UserId = r.UserId,
		Title = r.Title,
		OwnerName = r.OwnerName,
		Location = r.Location,
		Email = r.Email,
		PhoneNumber = r.PhoneNumber,
		Experiences = r.Experiences!.Select(ExperienceProjections.Basic.Compile()).ToArray()
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
	Full = 1,

	/// <summary>
	/// Contém os campos básicos da entidade, juntamente de todas as experiências profissionais relacionadas, mas sem outros relacionamentos.
	/// </summary>
	Experiences = 2
}