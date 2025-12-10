namespace Resumi.Api.Data.Models;

public record UpdateCertificateModel : UpdateResumeNodeModel
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    public string? Type { get; init; }
}
