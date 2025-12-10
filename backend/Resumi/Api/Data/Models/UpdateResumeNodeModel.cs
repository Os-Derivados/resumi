using System.ComponentModel.DataAnnotations;
using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a atualização
/// de um <see cref="ResumeNode"/> dentro de um <see cref="Resume"/>.
/// </summary>
public abstract record UpdateResumeNodeModel
{
    [Required]
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? InstitutionName { get; init; }
    public string? Location { get; init; }
    public bool? IsRemote { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public bool? StillEngaged { get; init; }
}
