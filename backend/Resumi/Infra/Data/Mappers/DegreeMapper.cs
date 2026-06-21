using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class DegreeMapper : IEntityMapper<Degree, DegreeModel, AddDegreeRequest, UpdateDegreeRequest>
{
	public Degree? NewDomainModel(AddDegreeRequest dtoCreate)
	{
		try
		{
			return new Degree
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
				Highlights = dtoCreate.Highlights,
				Level = DegreeLevelExtensions.FromDisplayString(dtoCreate.Level)
			};
		}
		catch (DomainException)
		{
			return null;
		}
	}

	public DegreeModel? ToDto(Degree? entity)
	{
		try
		{
			if (entity is null) return null;

			return new DegreeModel
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
				Highlights = entity.Highlights,
				Level = entity.Level.ToDisplayString()
			};
		}
		catch
		{
			return null;
		}
	}

	public Degree? UpdatedDomainModel(UpdateDegreeRequest? dtoUpdate, Degree? entity)
	{
		try
		{
			if (entity is null || dtoUpdate is null) return null;

			var shallowCopy = entity.ShallowCopy();

			shallowCopy.Name = dtoUpdate.Name ?? shallowCopy.Name;
			shallowCopy.Description = dtoUpdate.Description ?? shallowCopy.Description;
			shallowCopy.InstitutionName = dtoUpdate.InstitutionName ?? shallowCopy.InstitutionName;
			shallowCopy.Location = dtoUpdate.Location ?? shallowCopy.Location;
			shallowCopy.IsRemote = dtoUpdate.IsRemote ?? shallowCopy.IsRemote;
			shallowCopy.StartDate = dtoUpdate.StartDate ?? shallowCopy.StartDate;
			shallowCopy.EndDate = dtoUpdate.EndDate ?? shallowCopy.EndDate;
			shallowCopy.StillEngaged = dtoUpdate.StillEngaged ?? shallowCopy.StillEngaged;
			shallowCopy.Highlights = dtoUpdate.Highlights ?? shallowCopy.Highlights;
			shallowCopy.Level = DegreeLevelExtensions.TryGetValue(dtoUpdate.Level, out var level)
				? level!.Value
				: shallowCopy.Level;

			return shallowCopy;
		}
		catch (DomainException)
		{
			return null;
		}
	}
}