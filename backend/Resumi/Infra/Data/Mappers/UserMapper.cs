using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class UserMapper : IUserMapper
{
    public AppUser? NewDomainModel(CreateUserModel dtoCreate)
    {
        return new AppUser
        {
            FullName = dtoCreate.FullName,
            Email = dtoCreate.Email,
            UserName = dtoCreate.Email,
            PhoneNumber = dtoCreate.PhoneNumber
        };
    }

    public AppUser? UpdatedDomainModel(UpdateUserModel? dtoUpdate, AppUser? entity)
    {
        throw new NotImplementedException();
    }

    public UserModel? ToDto(AppUser? entity)
    {
        if (entity is null) return null;

        return new UserModel()

        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber
        };
    }
}