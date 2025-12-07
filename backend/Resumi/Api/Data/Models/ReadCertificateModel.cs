namespace Resumi.Api.Data.Models;

public record ReadCertificateModel : ReadResumeNodeModel
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    public string? Type { get; init; }
}