using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class ResumeValidator : IDomainValidator<Resume>
{
    public Result<Resume> ValidateCreation(Resume? newResume)
    {
        throw new NotImplementedException();
    }

    public Result<Resume> ValidateSearch(Resume? targetResume)
    {
        throw new NotImplementedException();
    }

    public Result<Resume> ValidateUpdate(Resume? current, Resume? updated)
    {
        throw new NotImplementedException();
    }

    public Result<Resume> ValidateDeletion(Resume? targetResume)
    {
        throw new NotImplementedException();
    }
}