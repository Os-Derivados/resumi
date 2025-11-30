using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

/// <summary>
/// Este contrato representa um item de uma seção dentro de um <see cref="Resume"/>.
/// </summary>
public abstract class ResumeNode : Entity
{
    [Required]
    [ForeignKey(nameof(Resume))]
    public int ResumeId { get; set; }
    public Resume? Resume { get; set; }

    [Required]
    [StringLength(128)]
    public string? Name { get; set; }

    [Required]
    [StringLength(256)]
    public string? Description { get; set; }

    /// <summary>
    /// Representa o nome da empresa, instituição de ensino ou ONG associada a <see cref="Experience"/>,
    /// <see cref="AcademicDegree"/> ou <see cref="VolunteerExperience"/>, respectivamente.
    /// </summary>
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
