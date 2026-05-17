using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;

namespace Resumi.Infra.Parameters;

/// <summary>
/// Objeto de parâmetros para alterar consultas somente-leitura para entidades <see cref="Resume"/>  
/// </summary>
public struct ResumeIncludes
{
	/// <summary>
	/// Entidade sem relações.
	/// </summary>
	public static Expression<Func<Resume, ResumeModel>> Basic = r => new ResumeModel
	{
		Id = r.Id,
		Title = r.Title,
		OwnerName = r.OwnerName,
		Location = r.Location,
		Email = r.Email,
		PhoneNumber = r.PhoneNumber
	};

	/// <summary>
	/// Entidade com todas as relações incluídas.
	/// </summary>
	public static Expression<Func<Resume, ResumeModel>> Full = r => new ResumeModel
	{
		Id = r.Id,
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