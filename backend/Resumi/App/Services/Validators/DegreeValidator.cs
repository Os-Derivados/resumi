using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class DegreeValidator : IDomainValidator<Degree>
{
    public Result<Degree> ValidateCreation(Degree? newDegree)
    {
        throw new NotImplementedException();
    }

    public Result<Degree> ValidateSearch(Degree? targetDegree)
    {
        throw new NotImplementedException();
    }

    public Result<Degree> ValidateUpdate(Degree? current, Degree? updated)
    {
        throw new NotImplementedException();
    }

    public Result<Degree> ValidateDeletion(Degree? targetDegree)
    {
        throw new NotImplementedException();
    }
}