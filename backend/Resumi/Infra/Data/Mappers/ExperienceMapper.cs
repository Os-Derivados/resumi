using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Mappers;

public class
	ExperienceMapper : IEntityMapper<Experience, ExperienceModel, AddExperienceRequest, UpdateExperienceRequest>
{
	public Experience? NewDomainModel(AddExperienceRequest dtoCreate)
	{
		try
		{
			return new Experience
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
				Highlights = dtoCreate.Highlights
			};
		}
		catch (DomainException)
		{
			return null;
		}
	}

	public ExperienceModel? ToDto(Experience? entity)
	{
		try
		{
			if (entity is null) return null;

			return new ExperienceModel
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
				Highlights = entity.Highlights
			};
		}
		catch (InfrastructureException)
		{
			return null;
		}
	}

	public Experience? UpdatedDomainModel(UpdateExperienceRequest? dtoUpdate, Experience? entity)
	{
		try
		{
			if (dtoUpdate is null || entity is null) return null;

			var copy = entity.ShallowCopy();

			copy.Name = dtoUpdate.Name ?? copy.Name;
			copy.Description = dtoUpdate.Description ?? copy.Description;
			copy.InstitutionName = dtoUpdate.InstitutionName ?? copy.InstitutionName;
			copy.Location = dtoUpdate.Location ?? copy.Location;
			copy.IsRemote = dtoUpdate.IsRemote ?? copy.IsRemote;
			copy.StartDate = dtoUpdate.StartDate ?? copy.StartDate;
			copy.EndDate = dtoUpdate.EndDate ?? copy.EndDate;
			copy.StillEngaged = dtoUpdate.StillEngaged ?? copy.StillEngaged;
			copy.Highlights = dtoUpdate.Highlights ?? copy.Highlights;

			return copy;
		}
		catch (DomainException)
		{
			return null;
		}
	}
}