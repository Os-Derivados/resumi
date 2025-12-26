using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class UserValidator : IDomainValidator<AppUser>
{
    public Result<AppUser> ValidateCreation(AppUser? newEntity)
    {
        throw new NotImplementedException();
    }

    public Result<AppUser> ValidateSearch(AppUser? targetEntity)
    {
        throw new NotImplementedException();
    }

    public Result<AppUser> ValidateUpdate(AppUser? current, AppUser? updated)
    {
        throw new NotImplementedException();
    }

    public Result<AppUser> ValidateDeletion(AppUser? targetEntity)
    {
        throw new NotImplementedException();
    }
}