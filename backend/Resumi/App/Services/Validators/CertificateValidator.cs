using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class CertificateValidator : IDomainValidator<Certificate>
{
    public Result<Certificate> ValidateCreation(Certificate? newCertificate)
    {
        if (newCertificate is null)
            return Result<Certificate>.Failure("certificate", "Certificate is required.");

        var errors = new ResultDictionary();

        if (newCertificate.ResumeId <= 0)
            errors.AddError("resumeId", "Resume ID must be a positive integer.");

        if (string.IsNullOrWhiteSpace(newCertificate.Name))
            errors.AddError("name", "Name is required.");

        if (string.IsNullOrWhiteSpace(newCertificate.Description))
            errors.AddError("description", "Description is required.");

        if (string.IsNullOrWhiteSpace(newCertificate.InstitutionName))
            errors.AddError("institutionName", "Institution name is required.");

        if (newCertificate.StartDate == default)
            errors.AddError("startDate", "Start date is required.");

        if (newCertificate.EndDate.HasValue && newCertificate.EndDate < newCertificate.StartDate)
            errors.AddError("endDate", "End date cannot be before start date.");

        if (errors.Any())
            return Result<Certificate>.Failure(errors);

        return Result<Certificate>.Success(newCertificate);
    }

    public Result<Certificate> ValidateSearch(Certificate? targetCertificate)
    {
        if (targetCertificate is null || targetCertificate.Id <= 0)
            return Result<Certificate>.Failure("id", "Certificate ID is required for search.");

        return Result<Certificate>.Success(targetCertificate);
    }

    public Result<Certificate> ValidateUpdate(Certificate? current, Certificate? updated)
    {
        if (current is null || updated is null)
            return Result<Certificate>.Failure("certificate", "Certificate update requires current and updated entities.");

        var errors = new ResultDictionary();

        if (updated.StartDate == default)
            errors.AddError("startDate", "Start date is required.");

        if (updated.EndDate.HasValue && updated.EndDate < updated.StartDate)
            errors.AddError("endDate", "End date cannot be before start date.");

        if (errors.Any())
            return Result<Certificate>.Failure(errors);

        return Result<Certificate>.Success(updated);
    }

    public Result<Certificate> ValidateDeletion(Certificate? targetCertificate)
    {
        if (targetCertificate is null || targetCertificate.Id <= 0)
            return Result<Certificate>.Failure("id", "Certificate id is required for deletion.");

        return Result<Certificate>.Success(targetCertificate);
    }
}
