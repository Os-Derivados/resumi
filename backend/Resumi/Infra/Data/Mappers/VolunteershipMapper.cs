using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Mappers;

public class VolunteershipMapper : IEntityMapper<Volunteership, VolunteershipModel, AddVolunteershipRequest,
	UpdateVolunteershipRequest>
{
	public Volunteership? NewDomainModel(AddVolunteershipRequest dtoCreate)
	{
		try
		{
			return new Volunteership
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
			};
		}
		catch (Exception ex) when (ex is not DomainException)
		{
			return null;
		}
	}

	public VolunteershipModel? ToDto(Volunteership? entity)
	{
		try
		{
			if (entity is null) return null;

			return new VolunteershipModel
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
			};
		}
		catch (Exception ex) when (ex is not InfrastructureException)
		{
			return null;
		}
	}

	public Volunteership? UpdatedDomainModel(UpdateVolunteershipRequest? dtoUpdate, Volunteership? entity)
	{
		try
		{
			if (dtoUpdate is null || entity is null) return null;

			var shallowCopy = entity.ShallowCopy();

			shallowCopy.Name = dtoUpdate.Name ?? shallowCopy.Name;
			shallowCopy.Description = dtoUpdate.Description ?? shallowCopy.Description;
			shallowCopy.InstitutionName = dtoUpdate.InstitutionName ?? shallowCopy.InstitutionName;
			shallowCopy.Location ??= dtoUpdate.Location;
			shallowCopy.IsRemote = dtoUpdate.IsRemote ?? shallowCopy.IsRemote;
			shallowCopy.StartDate = dtoUpdate.StartDate ?? shallowCopy.StartDate;
			shallowCopy.EndDate ??= dtoUpdate.EndDate;
			shallowCopy.StillEngaged = dtoUpdate.StillEngaged ?? shallowCopy.StillEngaged;

			return null;
		}
		catch (Exception ex) when (ex is not DomainException)
		{
			return null;
		}
	}
}