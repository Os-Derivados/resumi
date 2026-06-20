using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Requests;

public record AddCertificateRequest : AddResumeNodeRequest
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    [Required] public required string Type { get; init; }
}
