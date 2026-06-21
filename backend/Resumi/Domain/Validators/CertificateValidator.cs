using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class CertificateValidator : IDomainValidator<Certificate>
{
    public static readonly string InvalidCreationModel = "O modelo de certificado fornecido é inválido.";
    public static readonly string InvalidPeriodRange = "A data de término deve ser posterior à data de início.";

    public static readonly string RequiredNotEngagedState =
        "Se o usuário não está mais durante o processo de certificação, a data de término é obrigatória.";

    public static readonly string NotFound = "Certificado não encontrado.";
    public static readonly string InvalidUpdateModel = "O modelo de certificado para atualização é inválido.";

    public Result ValidateCreation(Certificate? newEntity)
    {
        if (newEntity is null)
        {
            return Result.Failure(nameof(Certificate), InvalidCreationModel);
        }

        ResultErrors errors = [];

        if (newEntity is { StillEngaged: false, EndDate: null })
        {
            errors.AddError(nameof(Certificate), RequiredNotEngagedState);
        }

        if (newEntity.EndDate.HasValue && newEntity.EndDate < newEntity.StartDate)
        {
            errors.AddError(nameof(Certificate), InvalidPeriodRange);
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success;
    }

    public Result ValidateUpdate(Certificate? current, Certificate? updated)
    {
        if (current is null)
        {
            return Result.Failure(nameof(Certificate), NotFound);
        }

        if (updated is null)
        {
            return Result.Failure(nameof(Certificate), InvalidUpdateModel);
        }

        ResultErrors errors = [];

        if (updated is { StillEngaged: false, EndDate: null })
        {
            errors.AddError(nameof(Certificate), RequiredNotEngagedState);
        }

        if (updated.EndDate.HasValue && updated.EndDate < updated.StartDate)
        {
            errors.AddError(nameof(Certificate), InvalidPeriodRange);
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success;
    }

    public Result ValidateDeletion(Certificate? targetEntity)
    {
        return targetEntity is null
            ? Result.Failure(nameof(Certificate), NotFound)
            : Result.Success;
    }
}
