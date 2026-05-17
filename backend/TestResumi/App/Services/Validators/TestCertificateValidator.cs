using Resumi.App.Services.Validators;

namespace TestResumi.App.Services.Validators;

public class TestCertificateValidator
{
    [Fact]
    public void CreationShouldFailWhenRequiredFieldsMissing()
    {
        var validator = new CertificateValidator();
        var result = validator.ValidateCreation(null);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void CreationShouldFailWhenStartDateIsDefault()
    {
        var certificate = new Certificate
        {
            ResumeId = 1,
            Name = "Certificado teste",
            Description = "Descrição teste",
            InstitutionName = "Instituição",
            IsRemote = false,
            StartDate = default,
            StillEngaged = false,
            Type = CertificateType.Extracurricular,
        };

        var validator = new CertificateValidator();
        var result = validator.ValidateCreation(certificate);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void CreationShouldSucceedWithValidData()
    {
        var certificate = new Certificate
        {
            ResumeId = 1,
            Name = "Certificado teste",
            Description = "Descrição teste",
            InstitutionName = "Instituição",
            IsRemote = false,
            StartDate = DateTime.UtcNow,
            StillEngaged = true,
            Type = CertificateType.Extracurricular,
        };

        var validator = new CertificateValidator();
        var result = validator.ValidateCreation(certificate);

        Assert.True(result.Succeeded);
    }
}
