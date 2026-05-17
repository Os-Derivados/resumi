using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class ResumeValidator : IDomainValidator<Resume>
{
    private const int MaxTitleLength = 128;

    public static readonly string NotFound = "Currículo não encontrado.";

    public Result<Resume> ValidateCreation(Resume? newResume)
    {
        ResultDictionary errors = [];

        if (newResume is null)
        {
            errors.AddError(nameof(Resume), "O Currículo se encontra num estado inválido para cadastro.");
        }

        if (newResume is not null && newResume.Title.Length > MaxTitleLength)
        {
            errors.AddError(nameof(Resume.Title),
                $"O título do currículo não pode exceder {MaxTitleLength} caracteres.");
        }

        return errors.Count > 0
            ? Result<Resume>.Failure(errors)
            : Result<Resume>.Success(newResume!);
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