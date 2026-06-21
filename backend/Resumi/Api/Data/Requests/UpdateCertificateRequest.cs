namespace Resumi.Api.Data.Requests;

public record UpdateCertificateRequest : UpdateResumeNodeRequest
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    public string? Type { get; init; }
}
