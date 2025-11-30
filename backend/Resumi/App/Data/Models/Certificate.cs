using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

/// <summary>
/// Representa um certificado ou licença dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Certificates")]
public class Certificate : ResumeNode
{
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }

    [Required]
    [DefaultValue(CertificateType.Course)]
    public CertificateType Type { get; set; }
}
