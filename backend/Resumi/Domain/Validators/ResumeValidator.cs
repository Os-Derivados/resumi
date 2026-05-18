using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class ResumeValidator : IDomainValidator<Resume>
{
    private readonly string _titleExceedsMaxSize =
        $"O título do currículo não pode exceder {MaxTitleLength} caracteres.";

    private readonly string _ownerNameExceedsMaxSize =
        $"O nome do proprietário do currículo não pode exceder {MaxOwnerNameLength} caracteres.";

    private const int MaxTitleLength = 128;
    private const int MaxOwnerNameLength = 128;

    public Result<Resume> ValidateCreation(Resume? newResume)
    {
        ResultDictionary errors = [];

        if (newResume is null)
        {
            errors.AddError(nameof(Resume), Resume.InvalidState);
        }

        if (newResume is not null && newResume.Title.Length > MaxTitleLength)
        {
            errors.AddError(nameof(Resume.Title), _titleExceedsMaxSize);
        }

        return errors.Count > 0
            ? Result<Resume>.Failure(errors)
            : Result<Resume>.Success(newResume!);
    }

    public Result<Resume> ValidateUpdate(Resume? current, Resume? updated)
    {
        ResultDictionary errors = [];

        if (current is null)
        {
            errors.AddError(nameof(Resume), Resume.NotFound);
        }

        if (updated is null)
        {
            errors.AddError(nameof(Resume), Resume.InvalidState);
        }

        if (current is not null && updated is not null && current.Id != updated.Id)
        {
            errors.AddError(nameof(Resume), Entity.UpdatePrimaryKeyMismatch);
        }

        if (updated is not null && updated.Title.Length > MaxTitleLength)
        {
            errors.AddError(nameof(Resume), _titleExceedsMaxSize);
        }

        if (updated?.OwnerName?.Length > MaxOwnerNameLength)
        {
            errors.AddError(nameof(Resume), _ownerNameExceedsMaxSize);
        }

        return errors.Count > 0
            ? Result<Resume>.Failure(errors)
            : Result<Resume>.Success(updated!);
    }

    public Result<Resume> ValidateDeletion(Resume? targetResume)
    {
        ResultDictionary errors = [];

        if (targetResume is null)
        {
            errors.AddError(nameof(Resume), Resume.NotFound);
        }

        return errors.Count > 0
            ? Result<Resume>.Failure(errors)
            : Result<Resume>.Success(targetResume!);
    }
}