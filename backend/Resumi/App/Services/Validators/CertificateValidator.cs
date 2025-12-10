using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Validators;

public class CertificateValidator : IDomainValidator<Certificate>
{
    public Result<Certificate> ValidateCreation(Certificate? newCertificate)
    {
        throw new NotImplementedException();
    }

    public Result<Certificate> ValidateSearch(Certificate? targetCertificate)
    {
        throw new NotImplementedException();
    }

    public Result<Certificate> ValidateUpdate(Certificate? current, Certificate? updated)
    {
        throw new NotImplementedException();
    }

    public Result<Certificate> ValidateDeletion(Certificate? targetCertificate)
    {
        throw new NotImplementedException();
    }
}
