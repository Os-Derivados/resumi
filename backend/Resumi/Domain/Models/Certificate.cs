using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa um certificado ou licença dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Certificates")]
public class Certificate : ResumeNode
{
    public static readonly string FailedToCreate = "Não foi possível cadastrar certificado.";
    public static readonly string FailedToUpdate = "Não foi possível atualizar o certificado.";
    public static readonly string FailedToDelete = "Não foi possível deletar o certificado.";
    public static readonly string InternalError = "Ocorreu um erro interno ao processar certificado.";

    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }

    [Required]
    [DefaultValue(CertificateType.Course)]
    public CertificateType Type { get; set; }

    public override Certificate ShallowCopy()
    {
        return new Certificate
        {
            #region Campos de Entity

            Id = Id,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,

            #endregion

            #region Campos de ResumeNode

            ResumeId = ResumeId,
            Name = Name,
            Description = Description,
            InstitutionName = InstitutionName,
            Location = Location,
            IsRemote = IsRemote,
            StartDate = StartDate,
            EndDate = EndDate,
            StillEngaged = StillEngaged,

            #endregion

            #region Campos de Certificate

            Type = Type,
            CredentialId = CredentialId,
            CredentialUrl = CredentialUrl

            #endregion
        };
    }
}
