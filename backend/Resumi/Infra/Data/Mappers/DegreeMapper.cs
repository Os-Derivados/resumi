using Resumi.Api.Data.Models;
using Resumi.App.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class DegreeMapper : IEntityMapper<Degree, DegreeModel, AddDegreeModel, UpdateDegreeModel>
{
	public Degree? NewDomainModel(AddDegreeModel dtoCreate)
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

	public Degree? UpdatedDomainModel(UpdateDegreeModel? dtoUpdate, Degree? entity)
	{
		try
		{
			if (entity is null || dtoUpdate is null) return null;

			var shalowCopy = entity.ShallowCopy();

			shalowCopy.Name = dtoUpdate.Name ?? shalowCopy.Name;
			shalowCopy.Description = dtoUpdate.Description ?? shalowCopy.Description;
			shalowCopy.InstitutionName = dtoUpdate.InstitutionName ?? shalowCopy.InstitutionName;
			shalowCopy.Location = dtoUpdate.Location ?? shalowCopy.Location;
			shalowCopy.IsRemote = dtoUpdate.IsRemote ?? shalowCopy.IsRemote;
			shalowCopy.StartDate = dtoUpdate.StartDate ?? shalowCopy.StartDate;
			shalowCopy.EndDate = dtoUpdate.EndDate ?? shalowCopy.EndDate;
			shalowCopy.StillEngaged = dtoUpdate.StillEngaged ?? shalowCopy.StillEngaged;
			shalowCopy.Highlights = dtoUpdate.Highlights ?? shalowCopy.Highlights;
			shalowCopy.Level = DegreeLevelExtensions.TryGetValue(dtoUpdate.Level, out var level)
				? level!.Value
				: shalowCopy.Level;

			return shalowCopy;
		}
		catch (DomainException)
		{
			return null;
		}
	}
}