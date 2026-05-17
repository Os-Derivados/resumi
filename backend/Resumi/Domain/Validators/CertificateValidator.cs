using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class CertificateValidator : IDomainValidator<Certificate>
{
    public static readonly string InvalidCreationModel = "O modelo de certificado fornecido é inválido.";
    public static readonly string InvalidPeriodRange = "A data de término deve ser posterior à data de início.";

    public static readonly string RequiredNotEngagedState =
        "Se o usuário não está mais durante o processo de certificação, a data de término é obrigatória.";

    public static readonly string NotFound = "Certificado não encontrado.";
    public static readonly string InvalidUpdateModel = "O modelo de certificado para atualização é inválido.";

    public Result<Certificate> ValidateCreation(Certificate? newEntity)
    {
        if (newEntity is null)
        {
            return Result<Certificate>.Failure(nameof(Certificate), InvalidCreationModel);
        }

        ResultDictionary errors = [];

        if (newEntity is { StillEngaged: false, EndDate: null })
        {
            errors.AddError(nameof(Certificate), RequiredNotEngagedState);
        }

        if (newEntity.EndDate.HasValue && newEntity.EndDate < newEntity.StartDate)
        {
            errors.AddError(nameof(Certificate), InvalidPeriodRange);
        }

        return errors.Any() ? Result<Certificate>.Failure(errors) : Result<Certificate>.Success(newEntity);
    }

    public Result<Certificate> ValidateSearch(Certificate? targetEntity)
    {
        return targetEntity is null
            ? Result<Certificate>.Failure(nameof(Certificate), NotFound)
            : Result<Certificate>.Success(targetEntity);
    }

    public Result<Certificate> ValidateUpdate(Certificate? current, Certificate? updated)
    {
        if (current is null)
        {
            return Result<Certificate>.Failure(nameof(Certificate), NotFound);
        }

        if (updated is null)
        {
            return Result<Certificate>.Failure(nameof(Certificate), InvalidUpdateModel);
        }

        ResultDictionary errors = [];

        if (updated is { StillEngaged: false, EndDate: null })
        {
            errors.AddError(nameof(Certificate), RequiredNotEngagedState);
        }

        if (updated.EndDate.HasValue && updated.EndDate < updated.StartDate)
        {
            errors.AddError(nameof(Certificate), InvalidPeriodRange);
        }

        return errors.Any() ? Result<Certificate>.Failure(errors) : Result<Certificate>.Success(updated);
    }

    public Result<Certificate> ValidateDeletion(Certificate? targetEntity)
    {
        return targetEntity is null
            ? Result<Certificate>.Failure(nameof(Certificate), NotFound)
            : Result<Certificate>.Success(targetEntity);
    }
}
