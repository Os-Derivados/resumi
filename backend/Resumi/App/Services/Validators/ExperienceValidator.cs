using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class ExperienceValidator : IDomainValidator<Experience>
{
    public Result<Experience> ValidateCreation(Experience? newExperience)
    {
        throw new NotImplementedException();
    }

    public Result<Experience> ValidateSearch(Experience? targetExperience)
    {
        throw new NotImplementedException();
    }

    public Result<Experience> ValidateUpdate(Experience? current, Experience? updated)
    {
        throw new NotImplementedException();
    }

    public Result<Experience> ValidateDeletion(Experience? targetExperience)
    {
        throw new NotImplementedException();
    }
}