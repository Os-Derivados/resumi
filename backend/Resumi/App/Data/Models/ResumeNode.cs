using System.ComponentModel.DataAnnotations;

namespace Resumi.App.Data.Models;

/// <summary>
/// Este contrato representa um item de uma seção dentro de um <see cref="Resume"/>.
/// </summary>
public abstract class ResumeNode
{
    public int ResumeId { get; set; }
    public Resume? Resume { get; set; }

    [Required]
    [StringLength(128)]
    public string? Name { get; set; }

    [Required]
    [StringLength(256)]
    public string? Description { get; set; }

    [Required]
    [StringLength(128)]
    public string? InstitutionName { get; set; }

    [StringLength(64)]
    public string? Location { get; set; }

    [Required]
    public bool IsRemote { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public bool StillEngaged { get; set; }
}
