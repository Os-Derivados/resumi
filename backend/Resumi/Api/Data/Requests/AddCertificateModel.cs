using System.ComponentModel.DataAnnotations;
using Resumi.Api.Data.Models;

namespace Resumi.Api.Data.Requests;

public record AddCertificateModel : AddResumeNodeModel
{
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    [Required] public required string Type { get; init; }
}
