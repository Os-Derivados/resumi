using Resumi.Api.Data.Models;

namespace Resumi.Api.Data.Requests;

public record UpdateCertificateModel : UpdateResumeNodeModel
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    public string? Type { get; init; }
}
