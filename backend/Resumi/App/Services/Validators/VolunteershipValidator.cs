using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class VolunteershipValidator : IDomainValidator<Volunteership>
{
    public Result<Volunteership> ValidateCreation(Volunteership? newVolunteership)
    {
        throw new NotImplementedException();
    }

    public Result<Volunteership> ValidateSearch(Volunteership? targetVolunteership)
    {
        throw new NotImplementedException();
    }

    public Result<Volunteership> ValidateUpdate(Volunteership? current, Volunteership? updated)
    {
        throw new NotImplementedException();
    }

    public Result<Volunteership> ValidateDeletion(Volunteership? targetVolunteership)
    {
        throw new NotImplementedException();
    }
}