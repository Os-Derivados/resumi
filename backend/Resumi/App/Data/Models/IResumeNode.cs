using System.ComponentModel.DataAnnotations;

namespace Resumi.App.Data.Models;

/// <summary>
/// Este contrato representa um item de uma seção dentro de um <see cref="Resume"/>.
/// </summary>
public interface IResumeNode
{
    int ResumeId { get; set; }
    Resume? Resume { get; set; }

    [Required]
    [StringLength(128)]
    string? Name { get; set; }

    [Required]
    [StringLength(256)]
    string? Description { get; set; }

    [Required]
    [StringLength(128)]
    string? InstitutionName { get; set; }

    [StringLength(64)]
    string? Location { get; set; }

    [Required]
    bool IsRemote { get; set; }

    [Required]
    DateTime StartDate { get; set; }

    DateTime? EndDate { get; set; }

    [Required]
    bool StillEngaged { get; set; }
}
