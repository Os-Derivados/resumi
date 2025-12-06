using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class UserMapper : IUserMapper
{
    public AppUser? NewDomainModel(CreateUserModel? dtoCreate)
    {
        throw new NotImplementedException();
    }

    public AppUser? UpdatedDomainModel(UpdateUserModel? dtoUpdate, AppUser? entity)
    {
        throw new NotImplementedException();
    }

    public ReadUserModel? ToDto(AppUser? entity)
    {
        throw new NotImplementedException();
    }
}