using System.ComponentModel.DataAnnotations;
using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros genérico para a atribuição
/// de elementos de um <see cref="Resume"/>, como
/// <see cref="Experience"/>, <see cref="AcademicDegree"/> ou <see cref="VolunteerExperience"/>
/// </summary>
public abstract record AddNodeModel
{
    [Required]
    public int ResumeId { get; init; }

    [Required]
    [StringLength(128)]
    public string? Name { get; init; }

    [Required]
    [StringLength(256)]
    public string? Description { get; init; }

    [Required]
    [StringLength(128)]
    public string? InstitutionName { get; init; }

    [StringLength(64)]
    public string? Location { get; init; }

    [Required]
    public bool IsRemote { get; init; }

    [Required]
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    [Required]
    public bool StillEngaged { get; init; }
}
