using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros genérico para a atribuição
/// de elementos de um <see cref="Resume"/>, como
/// <see cref="Experience"/>, <see cref="Degree"/> ou <see cref="Volunteership"/>
/// </summary>
public abstract record AddResumeNodeModel
{
    [Required] public int ResumeId { get; init; }

    [Required] [StringLength(128)] public required string Name { get; init; }

    [Required] [StringLength(256)] public required string Description { get; init; }

    [Required] [StringLength(128)] public required string InstitutionName { get; init; }

    [StringLength(64)] public string? Location { get; init; }

    [Required] public bool IsRemote { get; init; }

    [Required] public required DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    [Required] public bool StillEngaged { get; init; }
}
