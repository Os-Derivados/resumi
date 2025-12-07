using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Models;

public record AddCertificateModel : AddResumeNodeModel
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    [Required] public string? Type { get; init; }
}
