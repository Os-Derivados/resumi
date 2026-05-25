using Resumi.Api.Data.Models;
using Resumi.App.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Mappers;

public class UserMapper : IEntityMapper<AppUser, UserModel, CreateUserModel, UpdateUserModel>
{
	public AppUser? NewDomainModel(CreateUserModel dtoCreate)
	{
		try
		{
			return new AppUser
			{
				FullName = dtoCreate.FullName,
				PhoneNumber = dtoCreate.PhoneNumber,
				Email = dtoCreate.Email
			};
		}
		catch (DomainException)
		{
			return null;
		}
	}

	public UserModel? ToDto(AppUser? entity)
	{
		try
		{
			if (entity is null) return null;

			return new UserModel
			{
				Id = entity.Id,
				FullName = entity.FullName,
				PhoneNumber = entity.PhoneNumber,
				Email = entity.Email
			};
		}
		catch (InfrastructureException)
		{
			return null;
		}
	}

	public AppUser? UpdatedDomainModel(UpdateUserModel? dtoUpdate, AppUser? entity)
	{
		try
		{
			if (entity is null || dtoUpdate is null) return null;

			var clone = entity.ShallowCopy();

			clone.FullName = dtoUpdate.FullName ?? clone.FullName;
			clone.PhoneNumber = dtoUpdate.PhoneNumber ?? clone.PhoneNumber;
			clone.Email = dtoUpdate.Email ?? clone.Email;

			return clone;
		}
		catch (DomainException)
		{
			return null;
		}
	}
}